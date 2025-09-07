using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : BaseStatsPanel
{
    public TMPro.TextMeshProUGUI tmpEquipmentLv;

    protected override void AssignCallback()
    {
        UserGameStatsData.OnUpgradeEquipment += OnCoinChangeCallback;
    }

    protected override void UnassignCallback()
    {
        UserGameStatsData.OnUpgradeEquipment -= OnCoinChangeCallback;
    }

    #region Callback

    private void OnCoinChangeCallback(int equipmentLv)
    {
        this.tmpEquipmentLv.SetText($"Equipment Lv: {equipmentLv + 1}");
    }

    #endregion
}
