using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plant or Animal on Plant Tile
/// </summary>
public class BaseCreatureItem : WorkableObject, IUpdateable
{
    protected int farmID;
    public float timer = 10f;
    public static System.Action<BaseCreatureItem> OnCreatureReturn2Pool;
    public static System.Action<BaseCreatureItem> OnCreatureEndCycle;
    public static System.Action<BaseCreatureItem> OnCreatureCollected;

    public int FarmID => this.farmID;
    public bool CanCollectCreature
    {
        get
        {
            return CurrentProductAmount > 0;
        }
    }
    protected virtual int CurrentProductAmount
    {
        get
        {
            if (creatureData == null)
            {
                return -1;
            }

            return creatureData.CurrentProductAmount;
        }
    }
    protected virtual int CollectedProductAmount
    {
        get
        {
            if (creatureData == null)
            {
                return -1;
            }

            return creatureData.CollectedProductAmount;
        }
    }

    protected GameCreatureData creatureData;

    protected CreatureStatsConfig creatureStatsConfig;

    public CreatureStatsConfig CreatureStatsConfig
    {
        get
        {
            if (this.creatureStatsConfig == null)
            {
                this.creatureStatsConfig = CreatureStatsConfigs.Instance.GetCreatureStatsConfig(this.objectType);
            }
            return this.creatureStatsConfig;
        }
    }

    public bool ReachMaxLifeTime
    {
        get
        {
            if (CreatureStatsConfig == null)
            {
                return true;
            }

            return (CurrentProductAmount + CollectedProductAmount) >= CreatureStatsConfig.CycleLifeCount;
        }
    }

    public int CreatureID => this.ObjectID;

    public override void DoInteractAction()
    {
        base.DoInteractAction();
        Debug.Log($"This is Creature {this.CreatureID}");
        ///Collected
        CollectedProduct();
    }

    public override void WorkerDoInteractAction()
    {
        base.WorkerDoInteractAction();
        Debug.LogError($"Worker collect product");
        DoInteractAction();
    }

    private void OnDisable()
    {
        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.UnregisterUpdateableObject(this);
        }
    }

    public virtual void OnUpdate(float dt)
    {
        timer -= dt;
        if (timer <= 0)
        {
            OnEndCycleDo();
        }
    }

    public virtual void InitData(GameCreatureData data)
    {
        this.creatureData = data;
        if (data == null)
        {
            Debug.LogError("Creature Data null, can not init data");
            return;
        }
        this.farmID = data.FarmID;
        CheckTimer();
        UpdateManager.Instance.UnregisterUpdateableObject(this);
        UpdateManager.Instance.RegisterUpdateableObject(this);
    }

    protected virtual void CheckTimer()
    {
        ///Get timer
        if (CreatureStatsConfig != null)
        {
            ///If reach limit life time
            if (ReachMaxLifeTime)
            {
                this.timer = CreatureStatsConfigs.Instance.TimeCreatureRottenBySec;
            }
            else
            {
                this.timer = CreatureStatsConfig.CycleTimeBySec;
                //TODO timer bonus
                this.timer *= 1f;
                //Check in case offline
                if (creatureData != null)
                {
                    var timeDataPass = this.creatureData.GetTimePassSinceStartTime();
                    //Debug.LogError($"Time Data Pass {timeDataPass}");
                    this.timer -= timeDataPass;
                }
            }
        }       
    }

    protected virtual void OnEndCycleDo()
    {
        Debug.LogError($"End Cycle {ObjectType}, FarmID: {farmID}");
        bool rotton = GameCreatureDatas.Instance.OnACreatureEndCycleDo(farmID);
        OnCreatureEndCycle?.Invoke(this);
        if (rotton)
        {
            Return2Pool();
            return;
        }
        Debug.LogError($"End Cycle {ObjectType}_farm_{farmID}: Current Product {CurrentProductAmount}, Collected Product {CollectedProductAmount}");
        CheckTimer();
    }

    protected virtual void CollectedProduct()
    {
        if (this.CurrentProductAmount > 0)
        {
            bool reachMaxLifeTime = GameCreatureDatas.Instance.OnCollectACreatureProductDo(farmID);
            OnCreatureCollected?.Invoke(this);
            if (reachMaxLifeTime)
            {
                Return2Pool();
                return;
            }
            Debug.LogError($"Collect {ObjectType}_farm_{farmID}: Current Product {CurrentProductAmount}, Collected Product {CollectedProductAmount}");
        }
    }

    protected virtual void Return2Pool()
    {
        CreaturesManager.Instance.Return2FreePool(this);
        this.gameObject.SetActive(false);
        this.creatureData = null;
        OnCreatureReturn2Pool?.Invoke(this);
    }
}
