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

        var neWorkerObj = Instantiate(workerObj);

        var workerActor = neWorkerObj.GetComponent<WorkerActor>();

        return workerActor;
    }
}
