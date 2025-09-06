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
    private int startIdCounter = 5000;

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

        var creatureDatasClone = CreatureDatas.GetCloneCreatureDatas();
        for (int i = 0; i < creatureDatasClone.Count; i++)
        {
            var creature = CreateCreatureItem(creatureDatasClone[i]);
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

        neCreature.StartTimer();
        OnCreateACreature?.Invoke(neCreature);
        return neCreature;
    }

    public BaseCreatureItem GetFreeCreatureItem(ItemType creatureType)
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

    private void AddCreature2ActivePool(BaseCreatureItem creatureItem)
    {
        if (creatureItem == null)
        {
            Debug.LogError("Can not add null creature to active pool");
            return;
        }
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

    public void Return2Pool(BaseCreatureItem item)
    {

    }


}
