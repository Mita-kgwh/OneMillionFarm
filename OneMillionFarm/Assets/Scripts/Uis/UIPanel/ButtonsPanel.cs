using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsPanel : BaseStatsPanel
{
    protected override void AssignCallback()
    {
        BaseDialog.OnShowDialog += OnShowDialogCallback;
        BaseDialog.OnHideDialog += OnHideDialogCallback;
    }

    protected override void UnassignCallback()
    {
        BaseDialog.OnShowDialog -= OnShowDialogCallback;
        BaseDialog.OnHideDialog -= OnHideDialogCallback;
    }

    public void Button_Show()
    {
        if (animating)
        {
            return;
        }
        AnimationShow();
    }

    public void Button_Hide()
    {
        if (animating)
        {
            return;
        }
        AnimationHide();
    }

    #region Callback
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
