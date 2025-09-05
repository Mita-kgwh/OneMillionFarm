using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoSingleton<GameDataManager>
{
    private GameWorkerDatas workerDatas;

    public GameWorkerDatas WorkerDatas
    {
        get 
        { 
            if (workerDatas == null)
            {
                workerDatas = new GameWorkerDatas();
                workerDatas.Init();
            }
            return workerDatas; 
        }
    }

    private UserGameCoinData coinData;

    public UserGameCoinData CoinData
    {
        get
        {
            if(coinData == null)
            {
                coinData = new UserGameCoinData();
                coinData.Init();
            }

            return coinData;
        }
    }

    public void Init()
    {
        workerDatas = new GameWorkerDatas();
        coinData = new UserGameCoinData();

        workerDatas.Init();
        coinData.Init();
    }

}
