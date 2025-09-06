using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameFarmTileData : BaseGameData
{
    public int farmTileID;
    public ItemType creatureType = ItemType.NONE;
    public int workerID;

    public int WorkerID => workerID;
    public int FarmTileID => farmTileID;
    public ItemType CreatureType => creatureType;

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
        data.creatureType = creatureType;
        data.farmTileID = farmTileID;

        return data;
    }

    public GameFarmTileData AssignCreature(ItemType creatureType)
    {
        this.creatureType = creatureType;
        return this;
    }

    public GameFarmTileData UnassignCreature()
    {
        this.creatureType = ItemType.NONE;
        return this;
    }

    public GameFarmTileData AssignWorker(int _workerId)
    {
        this.workerID = _workerId;
        return this;
    }

    public GameFarmTileData UnassignWorker()
    {
        this.workerID = -1;
        return this;
    }
}
