using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPanel : MonoBehaviour
{
    public TMPro.TextMeshProUGUI tmpCoin;

    private void Awake()
    {
        UnassignCallback();
        AssignCallback();
    }

    private void OnDestroy()
    {
        UnassignCallback();
    }

    private void AssignCallback()
    {
        UserGameCoinData.OnCoinChange += OnCoinChangeCallback;
    }

    private void UnassignCallback()
    {
        UserGameCoinData.OnCoinChange -= OnCoinChangeCallback;
    }

    #region

    private void OnCoinChangeCallback(int curCoin, int amountChange)
    {
        this.tmpCoin.SetText($"{curCoin}");
    }

    #endregion
}
