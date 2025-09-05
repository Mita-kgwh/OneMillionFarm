using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoSingleton<WorkerManager>
{
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

        var newWorkerData = WorkerDatas.BuyWorker();

        if (newWorkerData == null)
        {
            return null;
        }

        var neWorker = SpawnObjectManager.Instance.CreateWorker();
        neWorker.SetUpWorker(newWorkerData.WorkerID);

        workerActors.Add(neWorker);
        OnCreateAWorker?.Invoke(neWorker);
        return neWorker;
    }

    public WorkerActor GetFreeWorker()
    {
        var freeWorkerData = WorkerDatas.GetFreeWorker();
        
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
