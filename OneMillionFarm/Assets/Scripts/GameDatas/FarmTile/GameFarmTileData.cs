using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameFarmTileData : BaseGameData
{
    public int farmTileID;
    //public ItemType creatureType = ItemType.NONE;

    public int FarmTileID => farmTileID;
    //public ItemType CreatureType => creatureType;


    public GameFarmTileData() { }

    public GameFarmTileData(int _farmID) { this.farmTileID = _farmID; }

    public GameFarmTileData Clone()
    {
        var data = new GameFarmTileData();

        //data.creatureType = creatureType;
        data.farmTileID = farmTileID;

        return data;
    }

    //public GameFarmTileData AssignCreature(ItemType creatureType)
    //{
    //    this.creatureType = creatureType;
    //    return this;
    //}

    //public GameFarmTileData UnassignCreature()
    //{
    //    this.creatureType = ItemType.NONE;
    //    return this;
    //}
}
