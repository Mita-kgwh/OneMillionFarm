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
    ///Use both of this to check after offline
    /// <summary>
    /// amount of product this creature having now
    /// </summary>
    public int currentAmountProduct;
    /// <summary>
    /// amount of product have collected
    /// </summary>
    public int collectedProductAmount;
    public ItemType CreatureType => this.creatureType;

    [System.NonSerialized]
    private CreatureStatsConfig creatureStatsConfig;

    public int FarmID => this.farmID;

    public GameCreatureData() { }
    public GameCreatureData(ItemType itemType, int farmID) 
    {
        this.creatureType = itemType;
        this.farmID = farmID;
    }

    public GameCreatureData Clone()
    {
        var clone = new GameCreatureData();

        clone.creatureType = this.creatureType; 
        clone.farmID = this.farmID; 
        clone.startTime = this.startTime;
        clone.currentAmountProduct = this.currentAmountProduct;
        clone.collectedProductAmount = this.collectedProductAmount;

        return clone;
    }

    public GameCreatureData SetStartTime(float startTime)
    {
        this.startTime = startTime;
        return this;
    }
}
