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

    public int StartWorkerAmount => this.startWorkerAmount;
    public int StartFarmTileAmount => this.startFarmTileAmount;
    public int MaxColumnFarmTile => this.maxColFarmTile;
    public int StorageSize => this.storageSize;

    public int startWorkerAmount = 1;
    public int startFarmTileAmount = 3;
    public int maxColFarmTile = 5;
    public int startEquipmentLv = 1;
    public int storageSize = 21;
    [Header("Items Starter")]
    public List<ItemTypeAmount> itemAmounts = new List<ItemTypeAmount>();

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

    public ItemTypeAmount AddAmount(int _amount)
    {
        this.amount += _amount;
        if (this.amount <= 0)
        {
            this.amount = 0;
        }

        return this;
    }
}
