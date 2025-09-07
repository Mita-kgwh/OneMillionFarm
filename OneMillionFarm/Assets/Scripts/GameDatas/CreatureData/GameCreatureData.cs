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
    public int currentProductAmount;
    /// <summary>
    /// amount of product have collected
    /// </summary>
    public int collectedProductAmount;
    public ItemType CreatureType => this.creatureType;

    [System.NonSerialized]
    private CreatureStatsConfig creatureStatsConfig;

    public CreatureStatsConfig CreatureStatsConfig
    {
        get
        {
            if (this.creatureStatsConfig == null)
            {
                this.creatureStatsConfig = CreatureStatsConfigs.Instance.GetCreatureStatsConfig(creatureType);
            }
            return this.creatureStatsConfig;
        }
    }

    public int FarmID => this.farmID;
    public int CurrentProductAmount => this.currentProductAmount;
    public int CollectedProductAmount => this.collectedProductAmount;

    public bool ReachMaxLifeTime
    {
        get
        {
            if (CreatureStatsConfig == null)
            {
                return true;
            }

            return (currentProductAmount + collectedProductAmount) >= CreatureStatsConfig.CycleLifeCount;
        }
    }
    public GameCreatureData() { }
    public GameCreatureData(ItemType itemType, int farmID) 
    {
        this.creatureType = itemType;
        this.farmID = farmID;
        SetStartTime();
    }

    public GameCreatureData Clone()
    {
        var clone = new GameCreatureData();

        clone.creatureType = this.creatureType; 
        clone.farmID = this.farmID; 
        clone.startTime = this.startTime;
        clone.currentProductAmount = this.currentProductAmount;
        clone.collectedProductAmount = this.collectedProductAmount;

        return clone;
    }

    public GameCreatureData SetStartTime()
    {
        this.startTime = GameUltis.GetLocalLongTime();
        return this;
    }

    /// <summary>
    /// Return true if be rotton, false if not n add more product
    /// </summary>
    /// <returns></returns>
    public bool OnCreatureEndCycleDo()
    {
        //Already reach max
        if (ReachMaxLifeTime)
        {
            //Check if rotten
            float rottenLimitTime = CreatureStatsConfigs.Instance?.TimeCreatureRottenBySec ?? 3600f;
            float timerRotten = GameUltis.GetLocalLongTime() - this.startTime;

            //Something wrong if <= 0 so remove it anyway
            //Or
            //Minus one to make sure it exceed the limit
            return timerRotten <= 0 || timerRotten >= (rottenLimitTime - 1f);
        }
        else // Not reach max yet
        {
            this.currentProductAmount++;
            SetStartTime();
            return false;
        }
    }

    public GameCreatureData OnCollectProductDo()
    {
        this.collectedProductAmount += this.currentProductAmount;
        this.currentProductAmount = 0;
        return this;
    }
}
