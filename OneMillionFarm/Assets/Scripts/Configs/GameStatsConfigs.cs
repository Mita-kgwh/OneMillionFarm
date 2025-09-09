using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/GameStatsConfigs", fileName = "GameStatsConfigs")]
public class GameStatsConfigs : ScriptableObject
{
    public static GameStatsConfigs Instance
    {
        get 
        {
            return GameDataManager.Instance.StatsConfigs;
        }
    }

    public int coinWinGame = 1000000;
    public int starterCoin = 0;
    public int startWorkerAmount = 1;
    public int startFarmTileAmount = 3;
    public int maxColFarmTile = 5;
    public int startEquipmentLv = 0;
    public int maxEquipmentLevel = 6;
    //Decrease time cycle of creature by percent
    public float equipmentBoost = 10f;
    //60% Boost 
    public int costUpgradeEquipment = 500;
    public int storageSize = 21;
    [Header("Items Starter")]
    public List<ItemTypeAmount> itemAmounts = new List<ItemTypeAmount>();
    public int StartWorkerAmount => this.startWorkerAmount;
    public int StartFarmTileAmount => this.startFarmTileAmount;
    public int MaxColumnFarmTile => this.maxColFarmTile;
    public int StorageSize => this.storageSize;
    public int StarterCoin => this.starterCoin;
    public int StartEquipmentLv => this.startEquipmentLv;
    public float EquipmentBoost => this.equipmentBoost;
    public int MaxEquipmentLevel => this.maxEquipmentLevel;
    public int CostUpgradeEquipment => this.costUpgradeEquipment;
    public int CoinWinGame => this.coinWinGame;

    public List<ItemTypeAmount> ItemAmountClone()
    {
        var results = new List<ItemTypeAmount>();

        for (int i = 0; i < itemAmounts.Count; i++)
        {
            results.Add(itemAmounts[i].Clone());
        }

        return results;
    }
}

[System.Serializable]
public class ItemTypeAmount
{
    public ItemType itemType;
    public int amount;

    public int Amount => amount;
    public ItemType ItemType => itemType;

    public ItemTypeAmount()
    {
        this.itemType = ItemType.NONE;
        this.amount = 0;
    }
    public ItemTypeAmount(ItemType itemType, int amount)
    {
        this.itemType = itemType;
        this.amount = amount;
    }

    public ItemTypeAmount Clone()
    {
        var clone = new ItemTypeAmount();
        clone.itemType = this.itemType;
        clone.amount = this.amount;
        return clone;
    }

    public ItemTypeAmount SetType(ItemType _itemType, int _amount = 1)
    {
        this.itemType = _itemType;
        this.amount = _amount;
        return this;
    }

    public ItemTypeAmount AddAmount(int _amount)
    {
        this.amount += _amount;
        if (this.amount <= 0)
        {
            Clear();
        }

        return this;
    }

    public ItemTypeAmount Clear()
    {
        this.amount = 0;
        this.itemType = ItemType.NONE;

        return this;
    }
}
