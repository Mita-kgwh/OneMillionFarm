using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoSingleton<WorkerManager>
{
    public Transform workerContain;
    public List<WorkerActor> workerActors;
    [SerializeField] private GameWorkersConfigs gameWorkersConfigs;
    private GameWorkerDatas gameWorkerDatas;

    private GameWorkerDatas GameWorkerDatas
    {
        get
        {
            if (gameWorkerDatas == null)
            {
                gameWorkerDatas = GameWorkerDatas.Instance;
            }

            return gameWorkerDatas;
        }
    }

    private Dictionary<int, WorkerActor> dicFindWorker = new Dictionary<int, WorkerActor>();

    public int CostBuyWorker => this.gameWorkersConfigs?.CostBuyWorker ?? 500;
    public float TimeWorkerDoAction => this.gameWorkersConfigs?.TimeWorkerDoAction ?? 120f;
    public static System.Action<WorkerActor> OnCreateAWorker;

    public void StartGame()
    {
        if (GameWorkerDatas == null)
        {
            Debug.LogError("Worker Datas Null, can not start game");
            return;
        }

        var workerDatas = GameWorkerDatas.WorkerDatas;
        var offset = Vector3.forward;
        for (int i = 0; i < workerDatas.Count; i++)
        {
            var worker = CreateWorker(workerDatas[i]);
            worker.transform.localPosition += offset;
            offset += Vector3.right * 1f;
        }
    }

    public void RestartGame()
    {
        var spawnManager = SpawnObjectManager.Instance;
        for (int i = 0; i < workerActors.Count; i++)
        {
            spawnManager.Return2Pool(workerActors[i]);
        }

        workerActors.Clear();
        gameWorkerDatas = null;
        dicFindWorker.Clear();
    }


    #region 

    public WorkerActor BuyWorker()
    {
        var coinData = UserGameStatsData.Instance;
        if (coinData == null)
        {
            return null;
        }

        if (!coinData.IsCanUse(CostBuyWorker))
        {
            return null;
        }
        coinData.UseCoin(CostBuyWorker);
        var newWorkerData = GameWorkerDatas.AddWorkerData();

        if (newWorkerData == null)
        {
            return null;
        }

        var neWorker = CreateWorker(newWorkerData);
        neWorker.transform.localPosition += Vector3.forward;
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
        if (neWorker == null)
        {
            Debug.LogError("SpawnObjectManager create null worker");
            return null;
        }
        neWorker.SetUpWorker(workerData.WorkerID);
        var workerTf = neWorker.transform;
        workerTf.SetParent(workerContain);
        workerTf.localRotation = Quaternion.identity;
        workerTf.localPosition = Vector3.zero;
        neWorker.gameObject.name = $"Worker_{workerData.WorkerID}";

        workerActors.Add(neWorker);
        OnCreateAWorker?.Invoke(neWorker);

        return neWorker;
    }

    public WorkerActor GetFreeWorker()
    {
        var freeWorkerData = GameWorkerDatas.GetFreeWorkerData();
        
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
                if (workerActors[i].WorkerID == workerID)
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
