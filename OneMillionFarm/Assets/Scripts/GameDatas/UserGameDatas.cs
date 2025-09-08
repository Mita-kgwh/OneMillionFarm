using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class UserGameDatas : BaseGameData
{
    public GameWorkerDatas workerDatas;

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

    public GameFarmTileDatas farmTileDatas;

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

    public GameCreatureDatas creatureDatas;

    public GameCreatureDatas CreatureDatas
    {
        get
        {
            if (creatureDatas == null)
            {
                creatureDatas = new GameCreatureDatas();
                creatureDatas.Init();
            }

            return creatureDatas;
        }
    }

    public UserGameStatsData gameStatsData;

    public UserGameStatsData GameStatsData
    {
        get
        {
            if (gameStatsData == null)
            {
                gameStatsData = new UserGameStatsData();
                gameStatsData.Init();
            }

            return gameStatsData;
        }
    }

    public GameStorageItemDatas storageItemDatas;
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
        base.Init();
        CreateNewData();
    }

    private void CreateNewData()
    {
        workerDatas = new GameWorkerDatas();
        farmTileDatas = new GameFarmTileDatas();
        creatureDatas = new GameCreatureDatas();
        gameStatsData = new UserGameStatsData();
        storageItemDatas = new GameStorageItemDatas();


        workerDatas.Init();
        farmTileDatas.Init();
        creatureDatas.Init();
        gameStatsData.Init();
        storageItemDatas.Init();
    }

    public override void OpenGame()
    {
        base.OpenGame();

        WorkerDatas.OpenGame();
        FarmTileDatas.OpenGame();
        CreatureDatas.OpenGame();
        GameStatsData.OpenGame();
        StorageItemDatas.OpenGame();
    }
}
