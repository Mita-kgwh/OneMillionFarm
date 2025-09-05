using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorkerDatas : BaseGameData
{
    public static GameWorkerDatas Instance
    {
        get
        {
            var gameManager = GameDataManager.Instance;
            if (gameManager == null)
                return null;

            return gameManager.WorkerDatas;
        }
    }

    public List<GameWorkerData> workerDatas;

    public List<GameWorkerData> WorkerDatas
    {
        get
        {
            if (workerDatas == null)
            {
                Init();
            }
            return workerDatas;
        }
    }

    private int startId = 1000;

    public override void Init()
    {
        base.Init();
        workerDatas = new List<GameWorkerData>();
    }

    public GameWorkerData BuyWorker()
    {
        int newId = startId + workerDatas.Count;
        GameWorkerData workerData = new GameWorkerData(newId);

        workerDatas.Add(workerData);

        return workerData;
    }

    public GameWorkerData GetFreeWorker()
    {
        for (int i = 0; i < workerDatas.Count; i++)
        {
            if (workerDatas[i].IsFree)
            {
                return workerDatas[i];
            }
        }

        return null;
    }

    

}
