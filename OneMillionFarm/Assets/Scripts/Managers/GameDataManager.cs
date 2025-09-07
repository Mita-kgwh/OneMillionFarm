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

    private GameStorageItemDatas storageItemDatas;
    public GameStorageItemDatas StorageItemDatas
    {
        get
        {
            if (storageItemDatas == null)
            {
                storageItemDatas = new GameStorageItemDatas();
                storageItemDatas.Init();
            }

            return storageItemDatas;
        }
    }

    public override void Init()
    {
        StartCoroutine(IE_LoadData());
    }

    private IEnumerator IE_LoadData()
    {
        //TODO if have data
        //Load Data
        if (false)
        {
            LoadUserData();
            yield return new WaitForSeconds(0.2f);
            OnOpenGame();
        }
        else
        {
            //Else Create New
            CreateNewData();
        }

        yield return new WaitForSeconds(0.5f);
        OnLoadDataDone?.Invoke();
    }

    private void CreateNewData()
    {
        workerDatas = new GameWorkerDatas();
        farmTileDatas = new GameFarmTileDatas();
        creatureDatas = new GameCreatureDatas();
        coinData = new UserGameCoinData();
        storageItemDatas = new GameStorageItemDatas();


        workerDatas.Init();
        farmTileDatas.Init();
        creatureDatas.Init();
        coinData.Init();      
        storageItemDatas.Init();
    }

    private void LoadUserData()
    {
        //TODO
    }

    private void OnOpenGame()
    {
        workerDatas.OpenGame();
        farmTileDatas.OpenGame();
        creatureDatas.OpenGame();
        coinData.OpenGame();
        storageItemDatas.OpenGame();
    }
}
