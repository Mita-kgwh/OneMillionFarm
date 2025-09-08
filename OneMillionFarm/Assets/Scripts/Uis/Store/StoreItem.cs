using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] protected Button purchaseBtn;
    [SerializeField] protected Image imgIcon;
    public TMPro.TextMeshProUGUI tmpName;
    public TMPro.TextMeshProUGUI tmpPrice;
    public TMPro.TextMeshProUGUI tmpPurchaseAmount;
    private StoreItemConfig itemConfig;

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

    private void OnCoinChangeCallback(int curentCoin, int amountChange)
    {
        bool canBuy = itemConfig == null ? false : itemConfig.IsCanPurchase;
        this.tmpPrice.color = canBuy ? Color.green : Color.red;
        this.purchaseBtn.interactable = canBuy;
    }
    public void CheckPurchase()
    {
        OnCoinChangeCallback(0, 0);
    }

    public void InitConfig(StoreItemConfig _itemConfig)
    {
        this.itemConfig = _itemConfig;
        if (itemConfig == null)
        {
            return;
        }

        this.tmpName.SetText(itemConfig.NameItem);
        this.tmpPrice.SetText($"{itemConfig.TradingValue}");
        this.tmpPurchaseAmount.SetText($"{itemConfig.TradingAmount}");
        var cf = GameAssetsConfigs.Instance.GetGameAssetsConfig(this.itemConfig.TypeItem);
        if (cf != null)
        {
            this.imgIcon.sprite = cf.iconSpr;
        }
    }

    public void Button_PurchaseItem()
    {
        if (itemConfig == null)
        {
            Debug.LogError($"Item Config null");
            return;
        }

        itemConfig.TradingItem();
    }
}
