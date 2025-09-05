using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectManager : MonoSingleton<SpawnObjectManager>
{
    [SerializeField] private GameObjectConfigs objectConfigs;

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

        var workerActor = workerObj.GetComponent<WorkerActor>();

        return workerActor;
    }
}
