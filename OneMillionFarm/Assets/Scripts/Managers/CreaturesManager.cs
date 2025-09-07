using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreaturesManager : MonoSingleton<CreaturesManager>
{
    [SerializeField] private CreatureStatsConfigs creatureStatsConfigs;
    [SerializeField] private Transform creatureContainTf;
    private Dictionary<ItemType, Dictionary<int, BaseCreatureItem>> activeCreaturePools = new Dictionary<ItemType, Dictionary<int, BaseCreatureItem>>();
    private Dictionary<ItemType, Queue<BaseCreatureItem>> freeCreaturePools = new Dictionary<ItemType, Queue<BaseCreatureItem>>();

    public static System.Action<BaseCreatureItem> OnCreateACreature;
    private int startIdCounter = 1;

    private GameCreatureDatas creatureDatas;

    private GameCreatureDatas CreatureDatas
    {
        get
        {
            if (creatureDatas == null)
            {
                creatureDatas = GameCreatureDatas.Instance;
            }
            return creatureDatas;
        }
    }

    public CreatureStatsConfigs CreatureStatsConfigs => creatureStatsConfigs;

    public override void Init()
    {
        base.Init();
        UnassignCallback();
        AssignCallback();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnassignCallback();
    }

    private void AssignCallback()
    {

    }

    private void UnassignCallback()
    {

    }

    public void StartGame()
    {
        if (CreatureDatas == null)
        {
            Debug.LogError("Creature Datas Null, can not start game");
            return;
        }

        var creatureDatasClone = CreatureDatas.CreatureDatas;
        for (int i = 0; i < creatureDatasClone.Count; i++)
        {
            CreateCreatureItem(creatureDatasClone[i]);
        }
    }

    public BaseCreatureItem CreateCreatureItem(GameCreatureData creatureData)
    {
        if (creatureData == null)
        {
            Debug.LogError("Creature Data Null, can not create");
            return null;
        }
        ItemType creatureType = creatureData.CreatureType;
        BaseCreatureItem neCreature = GetFreeCreatureItem(creatureType);

        if (neCreature == null)
        {
            Debug.LogError($"Create creature type {creatureType} null");
            return null;
        }

        neCreature.InitData(creatureData);
        OnCreateACreature?.Invoke(neCreature);
        return neCreature;
    }

    private BaseCreatureItem GetFreeCreatureItem(ItemType creatureType)
    {
        if ((int)creatureType / 100 != 2)
        {
            Debug.LogError($"Invalid type {creatureType}, return null");
            return null; 
        }

        BaseCreatureItem neCreature = null;

        if (freeCreaturePools.TryGetValue(creatureType, out var freeQueue))
        {
            ///Get in Free
            if (!freeQueue.TryDequeue(out neCreature))
            {
                Debug.LogError($"Dequeue fail {creatureType}");
                return neCreature;
            }
            neCreature.gameObject.SetActive(true);
        }
        else
        {
            ///Creature new 
            neCreature = SpawnObjectManager.Instance.CreateCreature(creatureType);
            if (neCreature == null)
            {
                Debug.LogError($"SpawnObjectManager create null creature {creatureType}");
                return null;
            }
            neCreature.transform.SetParent(creatureContainTf);
            neCreature.SetObjectID(startIdCounter);
            startIdCounter++;
        }
        //Add to active
        AddCreature2ActivePool(neCreature);
        return neCreature;
    }

    public BaseCreatureItem GetCanCollectCreatureItem()
    {
        foreach (var subActiveDic in activeCreaturePools.Values)
        {
            foreach (var creature in subActiveDic.Values)
            {
                if (creature.CanCollectCreature)
                {
                    return creature;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Call when Create new obj or Get Obj in Free Pool(Have dequeued in free pool)
    /// </summary>
    /// <param name="creatureItem"></param>
    private void AddCreature2ActivePool(BaseCreatureItem creatureItem)
    {
        if (creatureItem == null)
        {
            Debug.LogError("Can not add null creature to active pool");
            return;
        }

        //Add to active dic
        Dictionary<int, BaseCreatureItem> activeDic;
        if (!activeCreaturePools.TryGetValue(creatureItem.ObjectType, out activeDic))
        {
            //This type doesnt have dic active
            activeDic = new Dictionary<int, BaseCreatureItem>();
            if (!activeCreaturePools.TryAdd(creatureItem.ObjectType, activeDic))
            {
                Debug.LogError($"Can not add {creatureItem.ObjectType} dic to dic active pool");
            }
        }

        //This type has dic active
        if (!activeDic.TryAdd(creatureItem.CreatureID, creatureItem))
        {
            Debug.LogError($"Can not add {creatureItem.name} creature to active pool");
        }
    }

    public void Return2FreePool(BaseCreatureItem creatureItem)
    {
        if (creatureItem == null)
        {
            Debug.LogError("Can not remove null creature in active pool");
            return;
        }

        var creatureType = creatureItem.ObjectType;

        //Remove from active dic
        Dictionary<int, BaseCreatureItem> activeDic;
        if (!activeCreaturePools.TryGetValue(creatureType, out activeDic))
        {
            //This type doesnt have dic active
            Debug.LogError($"this {creatureType} does not have dic active pool");
            return;
        }

        activeDic.Remove(creatureItem.CreatureID);

        //Add to free pool queue
        if (freeCreaturePools.TryGetValue(creatureType, out var freeQueue))
        {
            freeQueue.Enqueue(creatureItem);            
        }
        else //It have no queue
        {
            Debug.LogError($"This {creatureType} has no queue, create one");
            Queue<BaseCreatureItem> freeCreatureQueue = new Queue<BaseCreatureItem>();
            freeCreatureQueue.Enqueue(creatureItem);
            if (!freeCreaturePools.TryAdd(creatureType, freeCreatureQueue))
            {
                Debug.LogError($"Add queue {creatureType} fail, check again");
            }
        }
    }


}
