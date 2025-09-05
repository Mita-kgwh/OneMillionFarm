using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFarmTileDatas : BaseGameData
{
    public static GameFarmTileDatas Instance
    {
        get
        {
            var gameManager = GameDataManager.Instance;
            if (gameManager == null)
                return null;

            return gameManager.FarmTileDatas;
        }
    }

    public int maxColumnFarmTile = 5;
    public int MaxColumnFarmTile => this.maxColumnFarmTile;

    public List<GameFarmTileData> farmTileDatas;

    public List<GameFarmTileData> FarmTileDatas
    {
        get
        {
            if (farmTileDatas == null)
            {
                Init();
            }
            return farmTileDatas;
        }
    }

    public List<GameFarmTileData> GetCloneFarmTileDatas()
    {
        var results = new List<GameFarmTileData>();
        if (farmTileDatas == null)
            return results;

        for (int i = 0; i < farmTileDatas.Count; i++)
        {
            results.Add(farmTileDatas[i].Clone());
        }
        return results;
    }

    public override void Init()
    {
        base.Init();
        farmTileDatas = new List<GameFarmTileData>();
        var statsCf = GameStatsConfigs.Instance;
        if (statsCf == null)
        {
            return;
        }
        this.maxColumnFarmTile = statsCf.MaxColumnFarmTile;
        var startAmount = statsCf.StartFarmTileAmount;
        for (int i = 0; i < startAmount; i++)
        {
            AddFarmTileData();
        }
    }

    public GameFarmTileData AddFarmTileData()
    {
        int newId = farmTileDatas.Count;
        int row = newId / maxColumnFarmTile;
        int col = newId % maxColumnFarmTile;
        newId = row * 1000 + col;
        GameFarmTileData farmTileData = new GameFarmTileData(newId);

        farmTileDatas.Add(farmTileData);

        return farmTileData;
    }

    public GameFarmTileData GetFreeFarmTileData()
    {
        for (int i = 0; i < farmTileDatas.Count; i++)
        {
            if (farmTileDatas[i].IsFree)
            {
                return farmTileDatas[i];
            }
        }

        return null;
    }

}
