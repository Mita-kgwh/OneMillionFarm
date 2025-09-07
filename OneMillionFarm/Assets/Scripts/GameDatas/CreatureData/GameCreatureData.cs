using System;
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
    public long startTime;
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
        //Debug.LogError(DateTime.Now);
        return this;
    }

    /// <summary>
    /// Return true if it has rotton, false if not
    /// </summary>
    /// <returns></returns>
    public bool CalculateOffline()
    {
        //Debug.LogError("Start Cal Offline");
        float rottenLimitTime = CreatureStatsConfigs.Instance?.TimeCreatureRottenBySec ?? 3600f;
        float timeOffline = GameUltis.GetTimePassSinceTimeLongValue(this.startTime);
        //Max Product when Close game
        if (ReachMaxLifeTime)
        {
            return timeOffline >= rottenLimitTime;
        }

        //If not max, Calculate product offline
        //total cycle when close game
        int totalLifeCycle = this.currentProductAmount + this.collectedProductAmount;
        int limitCycleLifeCount = CreatureStatsConfig.CycleLifeCount;
        int lifeCycleLeft = limitCycleLifeCount - totalLifeCycle;
        //TODO Get bonus
        float cycleTime = CreatureStatsConfig.CycleTimeBySec * 1f;
        int cycleOfflineCount = Mathf.FloorToInt(timeOffline / cycleTime);
        
        //Check with limit to get real cycle
        cycleOfflineCount = lifeCycleLeft >= cycleOfflineCount ? cycleOfflineCount : lifeCycleLeft;
        //Add product offline
        this.currentProductAmount += cycleOfflineCount;
        //Update new start time after {cycleOfflineCount} Cycle
        int totalSecByCycleOffline = Mathf.RoundToInt(cycleOfflineCount * cycleTime);
        this.startTime = GameUltis.AddTimeSecond2DateTime(this.startTime, totalSecByCycleOffline);
        timeOffline -= totalSecByCycleOffline;
        //Check Max Product after calculate profit offline
        if (ReachMaxLifeTime)
        {
            return timeOffline >= rottenLimitTime;
        }      

        return false;
    }

    public float GetTimePassSinceStartTime()
    {
        return GameUltis.GetTimePassSinceTimeLongValue(this.startTime);
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
            float timerRotten = GameUltis.GetTimePassSinceTimeLongValue(this.startTime);

            //Something wrong if <= 0 so remove it anyway
            //Or
            //Minus one to make sure it exceed the limit
            return timerRotten <= 0 || timerRotten >= (rottenLimitTime - 1f);
        }
        else // Not reach max yet
        {
            this.currentProductAmount++;
            //Debug.LogError($"Add product: {this.currentProductAmount}");
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
