using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class UserGameCoinData : BaseGameData
{
    public static UserGameCoinData Instance
    {
        get
        {
            var gameManager = GameDataManager.Instance;
            if (gameManager == null)
                return null;

            return gameManager.CoinData;
        }
    }

    public int currentCoin;
    private string COIN_KEY = "GAME_COIN";
    public static System.Action<int, int> OnCoinChange;
    public int CurrentCoin => currentCoin;

    public override void Init()
    {
        base.Init();
        AddCoin(0);
    }

    public override void OpenGame()
    {
        base.OpenGame();
        OnCoinChange?.Invoke(currentCoin, 0);
    }

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

    protected override void SaveData()
    {
        base.SaveData();
        PlayerPrefs.SetInt(COIN_KEY, currentCoin);
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
