using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEquipmentButton : BaseFeatureButton
{
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
