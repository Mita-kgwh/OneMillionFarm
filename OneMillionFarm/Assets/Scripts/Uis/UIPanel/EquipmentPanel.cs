using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : BaseStatsPanel
{
    public TMPro.TextMeshProUGUI tmpEquipmentLv;
    public TMPro.TextMeshProUGUI tmpBonusEquipment;

    protected override void AssignCallback()
    {
        BaseDialog.OnShowDialog += OnShowDialogCallback;
        BaseDialog.OnHideDialog += OnHideDialogCallback;
        UserGameStatsData.OnUpgradeEquipment += OnUpgradeEquipmentCallback;
    }

    protected override void UnassignCallback()
    {
        BaseDialog.OnShowDialog -= OnShowDialogCallback;
        BaseDialog.OnHideDialog -= OnHideDialogCallback;
        UserGameStatsData.OnUpgradeEquipment -= OnUpgradeEquipmentCallback;
    }

    #region Callback

    private void OnUpgradeEquipmentCallback(int equipmentLv, float boostBonus)
    {
        this.tmpEquipmentLv.SetText($"Level: {equipmentLv + 1}");
        this.tmpBonusEquipment.SetText($"-{boostBonus * 100f}%");
    }


    private void OnShowDialogCallback(BaseDialog baseDialog)
    {
        AnimationHide();
    }

    private void OnHideDialogCallback(BaseDialog baseDialog)
    {
        AnimationShow();
    }

    #endregion
}
