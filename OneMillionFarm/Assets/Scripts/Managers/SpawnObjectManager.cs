using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectManager : MonoSingleton<SpawnObjectManager>
{
    [SerializeField] private GameObjectConfigs objectConfigs;

    public BaseCreatureItem CreateCreature(ItemType creatureType)
    {
        if (objectConfigs == null)
        {
            return null;
        }

        var creatureObj = objectConfigs.GetObjectByType(creatureType);

        if (creatureObj == null)
        {
            return null;
        }

        var neCreatureObj = Instantiate(creatureObj);

        var baseCreatureItem = neCreatureObj.GetComponent<BaseCreatureItem>();

        return baseCreatureItem;
    }

    public WorkerActor CreateWorker()
    {
        if (objectConfigs == null)
        {
            return null;
        }

        var workerObj = objectConfigs.GetObjectByType(ItemType.WORKER);

        if (workerObj == null)
        {
            return null;
        }

        var neWorkerObj = Instantiate(workerObj);

        var workerActor = neWorkerObj.GetComponent<WorkerActor>();

        return workerActor;
    }
    
    public FarmTile CreateFarmTile()
    {
        if (objectConfigs == null)
        {
            return null;
        }

        var farmTileObj = objectConfigs.GetObjectByType(ItemType.FARMTILE);

        if (farmTileObj == null)
        {
            return null;
        }

        var neFarmTileObj = Instantiate(farmTileObj);

        var farmTile = neFarmTileObj.GetComponent<FarmTile>();

        return farmTile;
    }
}
