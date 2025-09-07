using System.Collections;
using System.Collections.Generic;
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

    public int CurrentCoin => currentCoin;

    public override void Init()
    {
        base.Init();
        if (PlayerPrefs.HasKey(COIN_KEY))
        {
            this.currentCoin = PlayerPrefs.GetInt(COIN_KEY, 0);
        }
        else
        {
            this.currentCoin = 0;
            SaveData();
        }
    }

    public bool IsCanUse(int amount)
    {
        return amount >= currentCoin;
    }

    public void AddCoin(int amount)
    {
        currentCoin += amount;
        SaveData();
    }

    public void UseCoin(int amount)
    {
        if (currentCoin < amount)
        {
            return;
        }
        currentCoin -= amount;
        SaveData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(COIN_KEY, currentCoin);
    }
}
