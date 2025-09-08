using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class UserGameStatsData : BaseGameData
{
    public static UserGameStatsData Instance
    {
        get
        {
            var gameManager = GameDataManager.Instance;
            if (gameManager == null)
                return null;

            return gameManager.GameStatsData;
        }
    }

    public int currentCoin;
    public int equipmentLv;
    //private string COIN_KEY = "GAME_COIN";
    public static System.Action<int, int> OnCoinChange;
    public static System.Action<int> OnUpgradeEquipment;
    public int CurrentCoin => this.currentCoin;
    public int EquipmentLv => this.equipmentLv;
    public float EquipmentBoost
    {
        get
        {
            return equipmentLv * GameStatsConfigs.EquipmentBoost;
        }
    }

    private GameStatsConfigs gameStatsConfigs;
    private GameStatsConfigs GameStatsConfigs
    {
        get
        {
            if (this.gameStatsConfigs == null)
            {
                this.gameStatsConfigs = GameDataManager.Instance.StatsConfigs;
            }

            return this.gameStatsConfigs;
        }
    }
    public override void Init()
    {
        base.Init();
        if (GameStatsConfigs != null)
        {
            AddCoin(this.gameStatsConfigs.StarterCoin);
        }
    }

    public override void OpenGame()
    {
        base.OpenGame();

        OnCoinChange?.Invoke(currentCoin, 0);
        OnUpgradeEquipment?.Invoke(equipmentLv);
    }
    #region COin
    public bool IsCanUse(int amount)
    {
        return amount <= currentCoin;
    }

    public void AddCoin(int amount)
    {
        currentCoin += amount;
        OnCoinChange?.Invoke(currentCoin, amount);
        SaveData();
    }

    public void UseCoin(int amount)
    {
        if (currentCoin < amount)
        {
            return;
        }
        currentCoin -= amount;
        OnCoinChange?.Invoke(currentCoin, -amount);
        SaveData();
    }
    #endregion

    #region Equipment

    public bool IsCanUpgradeEquipment()
    {        
        return IsCanUse(GameStatsConfigs.CostUpgradeEquipment);
    }

    public void UpgradeEquipment()
    {
        UseCoin(GameStatsConfigs.CostUpgradeEquipment);
        this.equipmentLv++;
        //Debug.LogError($"{equipmentLv}");
        OnUpgradeEquipment?.Invoke(equipmentLv);
        SaveData();
    }

    #endregion

    protected override void SaveData()
    {
        base.SaveData();
        //PlayerPrefs.SetInt(COIN_KEY, currentCoin);   
    }

#if UNITY_EDITOR

    [MenuItem("Cheat/Coins/Add 1K Coin")]
    public static void Editor_CheatAdd1KCoin()
    {
        Instance.AddCoin(1000);
    }
    [MenuItem("Cheat/Coins/Add 10K Coin")]
    public static void Editor_CheatAdd10KCoin()
    {
        Instance.AddCoin(10000);
    }
    [MenuItem("Cheat/Coins/Add 100K Coin")]
    public static void Editor_CheatAdd100KCoin()
    {
        Instance.AddCoin(100000);
    }
    [MenuItem("Cheat/Coins/Add 500K Coin")]
    public static void Editor_CheatAdd500KCoin()
    {
        Instance.AddCoin(500000);
    }

#endif
}
