using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPanel : BaseStatsPanel
{
    public TMPro.TextMeshProUGUI tmpCoin;

    protected override void AssignCallback()
    {
        UserGameStatsData.OnCoinChange += OnCoinChangeCallback;
    }

    protected override void UnassignCallback()
    {
        UserGameStatsData.OnCoinChange -= OnCoinChangeCallback;
    }

    #region Callback

    private void OnCoinChangeCallback(int curCoin, int amountChange)
    {
        this.tmpCoin.SetText($"Goal: {curCoin}/1M");
    }

    #endregion
}
