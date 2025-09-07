using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plant or Animal on Plant Tile
/// </summary>
public class BaseCreatureItem : BaseObject, IUpdateable
{
    protected int farmID;
    protected BaseCreatureBehaviour creatureBehaviour;
    public float timer = 10f;

    public static System.Action<BaseCreatureItem> OnReturn2Pool;

    protected virtual int CurrentProductAmount
    {
        get
        {
            if (CreatureData == null)
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
            if (CreatureData == null)
            {
                return -1;
            }

            return creatureData.CollectedProductAmount;
        }
    }

    protected GameCreatureData creatureData;

    protected GameCreatureData CreatureData
    {
        get 
        {
            if (creatureData == null)
            {
                //TODO
            }
            return creatureData; 
        }
    }

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

    public int CreatureID => this.objectID;

    public override void DoInteractAction()
    {
        base.DoInteractAction();
        Debug.Log($"This is Creature {this.objectID}");
        ///Collected
        CollectedProduct();
    }

    private void OnEnable()
    {
        //Prevent it call update when timer has not set yet 
        this.timer = 10f;
        UpdateManager.Instance.RegisterUpdateableObject(this);
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
            }
        }       
    }

    protected virtual void OnEndCycleDo()
    {
        Debug.LogError($"End Cycle {ObjectType}, FarmID: {farmID}");
        bool rotton = GameCreatureDatas.Instance.OnACreatureEndCycleDo(farmID);
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
            bool maxLifeTime = GameCreatureDatas.Instance.OnCollectACreatureProductDo(farmID);
            if (maxLifeTime)
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
        OnReturn2Pool?.Invoke(this);
    }
}
