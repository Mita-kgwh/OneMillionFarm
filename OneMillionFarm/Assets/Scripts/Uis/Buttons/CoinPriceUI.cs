using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPriceUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI tmpValue;
    private int value;

    private void OnEnable()
    {
        UnassignCallback();
        AssignCallback();
    }

    private void OnDisable()
    {
        UnassignCallback();
    }

    private void AssignCallback()
    {
        UserGameStatsData.OnCoinChange += OnCoinChangeCallback;
    }

    private void UnassignCallback()
    {
        UserGameStatsData.OnCoinChange -= OnCoinChangeCallback;
    }

    private void OnCoinChangeCallback(int curCoin, int amountChange)
    {
        this.tmpValue.color = curCoin >= value ? Color.green : Color.red;
    }

    public void InitValue(int _value)
    {
        this.value = _value;
        this.tmpValue.SetText($"{value}");
    }
}
