using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoSingleton<WorkerManager>
{
    public Transform workerContain;
    public List<WorkerActor> workerActors;

    private GameWorkerDatas workerDatas;

    private GameWorkerDatas WorkerDatas
    {
        get
        {
            if (workerDatas == null)
            {
                workerDatas = GameWorkerDatas.Instance;
            }

            return workerDatas;
        }
    }

    private Dictionary<int, WorkerActor> dicFindWorker = new Dictionary<int, WorkerActor>();

    private int costBuyWorker = 500;
    private float timeWorkerDoAction = 2f * 60f;
    public static System.Action<WorkerActor> OnCreateAWorker;

    public void StartGame()
    {
        if (WorkerDatas == null)
        {
            Debug.LogError("Worker Data Null");
            return;
        }

        var workerDatasClone = WorkerDatas.GetCloneWorkerDatas();
        var offset = Vector3.zero;
        for (int i = 0; i < workerDatasClone.Count; i++)
        {
            var worker = CreateWorker(workerDatasClone[i]);
            worker.transform.localPosition += offset;
            offset += Vector3.right * 1f;
        }
    }

    #region 

    public WorkerActor BuyWorker()
    {
        var coinData = UserGameCoinData.Instance;
        if (coinData == null)
        {
            return null;
        }

        if (!coinData.IsCanUse(costBuyWorker))
        {
            return null;
        }

        var newWorkerData = WorkerDatas.AddWorkerData();

        if (newWorkerData == null)
        {
            return null;
        }

        var neWorker = CreateWorker(newWorkerData);
       
        return neWorker;
    }

    private WorkerActor CreateWorker(GameWorkerData workerData)
    {
        if (workerData == null)
        {
            Debug.LogError("Worker Data null, can not create");
            return null;
        }

        var neWorker = SpawnObjectManager.Instance.CreateWorker();
        neWorker.SetUpWorker(workerData.WorkerID);
        var workerTf = neWorker.transform;
        workerTf.SetParent(workerContain);
        workerTf.localRotation = Quaternion.identity;
        workerTf.localPosition = Vector3.zero;

        workerActors.Add(neWorker);
        OnCreateAWorker?.Invoke(neWorker);

        return neWorker;
    }

    public WorkerActor GetFreeWorker()
    {
        var freeWorkerData = WorkerDatas.GetFreeWorkerData();
        
        if (freeWorkerData == null)
        { 

            return null; 
        }

        return GetWorkerById(freeWorkerData.WorkerID);
    }

    public WorkerActor GetWorkerById(int workerID)
    {
        WorkerActor targetWorker = null;
        if (!dicFindWorker.TryGetValue(workerID, out targetWorker))
        {
            for (int i = 0; i < workerActors.Count; i++)
            {
                if (workerActors[i].WorkerId == workerID)
                {
                    targetWorker = workerActors[i];
                    dicFindWorker.TryAdd(workerID, workerActors[i]);
                    break;
                }
            }
        }
        return targetWorker;
    }

    #endregion

    #region UI

    public void Button_BuyWorker()
    {
        BuyWorker();
    }

    #endregion
}
