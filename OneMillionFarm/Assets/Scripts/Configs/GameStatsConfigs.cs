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

    public int startWorkerAmount = 1;
    public int startFarmTileAmount = 3;
    public int startEquipmentLv = 1;
    [Header("Items Starter")]
    public List<ItemTypeAmount> itemAmounts = new List<ItemTypeAmount>();
}

[System.Serializable]
public class ItemTypeAmount
{
    public ItemType itemType;
    public int amount;
}
