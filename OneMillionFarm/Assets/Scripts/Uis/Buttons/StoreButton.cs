using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButton : BaseFeatureButton
{
    public override void Button_Click()
    {
        base.Button_Click();
        StoreDialog.DoShowDialog();
    }
}
