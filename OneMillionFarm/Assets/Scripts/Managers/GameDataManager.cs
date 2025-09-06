using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoSingleton<GameDataManager>
{
    [SerializeField] private GameStatsConfigs statsConfigs;

    public static System.Action OnLoadDataDone;

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

    private GameFarmTileDatas farmTileDatas;

    public GameFarmTileDatas FarmTileDatas
    {
        get
        {
            if (farmTileDatas == null)
            {
                farmTileDatas = new GameFarmTileDatas();
                farmTileDatas.Init();
            }

            return farmTileDatas;
        }
    }

    private GameCreatureDatas creatureDatas;

    public GameCreatureDatas CreatureDatas
    {
        get
        {
            if(creatureDatas == null)
            {
                creatureDatas = new GameCreatureDatas();
                creatureDatas.Init();
            }

            return creatureDatas;
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
        StartCoroutine(LoadData());
    }

    private IEnumerator LoadData()
    {
        //TODO if have data
        //Load Data
        //Else Create New
        CreateNewData();
        yield return new WaitForSeconds(0.5f);
        OnLoadDataDone?.Invoke();
    }

    private void CreateNewData()
    {
        workerDatas = new GameWorkerDatas();
        farmTileDatas = new GameFarmTileDatas();
        creatureDatas = new GameCreatureDatas();
        coinData = new UserGameCoinData();


        workerDatas.Init();
        farmTileDatas.Init();
        creatureDatas.Init();
        coinData.Init();      
    }
}
