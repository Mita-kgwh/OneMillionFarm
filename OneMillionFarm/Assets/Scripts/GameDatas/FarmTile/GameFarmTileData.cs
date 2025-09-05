using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameFarmTileData : BaseGameData
{
    public int workerID;

    public int WorkerID => workerID;

    public int farmTileID;

    public int FarmTileID => farmTileID;

    public bool IsFree
    {
        get
        {
            return workerID <= 0;
        }
    }

    public GameFarmTileData() { }

    public GameFarmTileData(int _farmID) { this.farmTileID = _farmID; }

    public GameFarmTileData Clone()
    {
        var data = new GameFarmTileData();

        data.workerID = workerID;
        data.farmTileID = farmTileID;

        return data;
    }
}
