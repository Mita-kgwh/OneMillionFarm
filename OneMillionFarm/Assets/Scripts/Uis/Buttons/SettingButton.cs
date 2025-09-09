using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : BaseFeatureButton
{
    public override void Button_Click()
    {
        base.Button_Click();
        SettingDialog.DoShowDialog();
    }
}
