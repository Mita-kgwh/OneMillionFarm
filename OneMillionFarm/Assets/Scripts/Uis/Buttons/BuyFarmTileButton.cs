using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyFarmTileButton : BaseFeatureButton
{
    [SerializeField] CoinPriceUI coinPriceUI;

    private void Awake()
    {
        if (coinPriceUI == null)
            return;

        int costBuyFarmTile = 500;
        var storeCf = StoreItemConfigs.Instance.GetStoreItemConfigByType(ItemType.FARMTILE);
        if (storeCf != null)
        {
            costBuyFarmTile = storeCf.TradingValue;
        }
        coinPriceUI.InitValue(costBuyFarmTile);
    }

    public override void Button_Click()
    {
        base.Button_Click();
        FarmTileManager.Instance.Button_BuyFarmTile();
    }
}
