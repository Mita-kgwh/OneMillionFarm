using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameCreatureData : BaseGameData
{
    public ItemType creatureType;

    public int farmID;

    /// <summary>
    /// Time start plant/raise
    /// </summary>
    public float startTime;
    public ItemType CreatureType => this.creatureType;

    public int FarmID => this.farmID;

    public GameCreatureData() { }

    public GameCreatureData Clone()
    {
        var clone = new GameCreatureData();

        clone.creatureType = this.creatureType; 
        clone.farmID = this.farmID; 
        clone.startTime = this.startTime;

        return clone;
    }

    public GameCreatureData SetStartTime(float startTime)
    {
        this.startTime = startTime;
        return this;
    }
}
