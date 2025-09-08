using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradeEquipmentButton : BaseFeatureButton
{
    [SerializeField] CoinPriceUI coinPriceUI;

    private void Awake()
    {
        if (coinPriceUI == null)
            return;

        int costBuyFarmTile = 500;
        var statsCf = GameStatsConfigs.Instance;
        if (statsCf != null)
        {
            costBuyFarmTile = statsCf.CostUpgradeEquipment;
        }
        coinPriceUI.InitValue(costBuyFarmTile);
    }

    public override void Button_Click()
    {
        var statsData = UserGameStatsData.Instance;
        if (statsData == null)
        {
            return;
        }

        if (statsData.IsCanUpgradeEquipment())
        {
            statsData.UpgradeEquipment();
        }
    }
}
