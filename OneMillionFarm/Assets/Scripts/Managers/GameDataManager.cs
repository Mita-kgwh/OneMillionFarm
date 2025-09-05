using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoSingleton<GameDataManager>
{
    [SerializeField] private GameStatsConfigs statsConfigs;

    public GameStatsConfigs StatsConfigs => statsConfigs;

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

    public override void Init()
    {
        LoadData();
    }

    private void LoadData()
    {
        //TODO if have data
        //Load Data
        //Else Create New
        CreateNewData();
    }

    private void CreateNewData()
    {
        workerDatas = new GameWorkerDatas();
        coinData = new UserGameCoinData();


        workerDatas.Init();
        coinData.Init();
    }
}
