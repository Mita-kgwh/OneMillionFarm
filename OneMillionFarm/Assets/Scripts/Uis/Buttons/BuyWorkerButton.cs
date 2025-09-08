using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyWorkerButton : BaseFeatureButton
{
    [SerializeField] CoinPriceUI coinPriceUI;

    private void Awake()
    {
        if (coinPriceUI == null)
            return;

        int costBuyFarmTile = 500;
        var storeCf = StoreItemConfigs.Instance.GetStoreItemConfigByType(ItemType.WORKER);
        if (storeCf != null)
        {
            costBuyFarmTile = storeCf.TradingValue;
        }
        coinPriceUI.InitValue(costBuyFarmTile);
    }
    public override void Button_Click()
    {
        base.Button_Click();
        WorkerManager.Instance.Button_BuyWorker();
    }
}
