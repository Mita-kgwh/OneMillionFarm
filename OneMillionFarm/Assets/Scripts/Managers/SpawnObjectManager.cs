using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectManager : MonoSingleton<SpawnObjectManager>
{
    [SerializeField] private GameObjectConfigs objectConfigs;

    //For Worker and Tile only
    private Dictionary<ItemType, Queue<BaseObject>> dicWaitObjectsPool = new Dictionary<ItemType, Queue<BaseObject>>();

    public void Return2Pool(BaseObject baseObject)
    {
        if (baseObject == null)
        {
            return;
        }
        Queue<BaseObject> waitQueue = null;
        if (!dicWaitObjectsPool.TryGetValue(baseObject.ObjectType, out waitQueue))
        {
            waitQueue = new Queue<BaseObject>();
            waitQueue.Enqueue(baseObject);
            dicWaitObjectsPool.TryAdd(baseObject.ObjectType, waitQueue);
        }
        else
        {
            waitQueue.Enqueue(baseObject);
        }
        baseObject.gameObject.SetActive(false);
    }

    public BaseCreatureItem CreateCreature(ItemType creatureType)
    {
        var neCreatureObj = GetGameObjectByType(creatureType);

        if (neCreatureObj == null)
        { 
            return null;         
        }

        var baseCreatureItem = neCreatureObj.GetComponent<BaseCreatureItem>();

        return baseCreatureItem;
    }

    public WorkerActor CreateWorker()
    {
        var neWorkerObj = GetGameObjectByType(ItemType.WORKER);

        if (neWorkerObj == null)
        { 
            return null; 
        }

        var workerActor = neWorkerObj.GetComponent<WorkerActor>();

        return workerActor;
    }
    
    public FarmTile CreateFarmTile()
    {
        var neFarmTileObj = GetGameObjectByType(ItemType.FARMTILE);

        if (neFarmTileObj == null) 
        { 
            return null; 
        }

        var farmTile = neFarmTileObj.GetComponent<FarmTile>();

        return farmTile;
    }

    private GameObject GetGameObjectByType(ItemType type)
    {
        if (dicWaitObjectsPool.TryGetValue(type, out var waitQueue))
        {
            if (waitQueue.TryDequeue(out var baseObj))
            {
                baseObj.gameObject.SetActive(true);
                return baseObj.gameObject;
            }
        }

        if (objectConfigs == null)
        {
            return null;
        }

        var gameObjPf = objectConfigs.GetObjectByType(type);

        if (gameObjPf == null)
        {
            return null;
        }

        return Instantiate(gameObjPf);
    }
}
