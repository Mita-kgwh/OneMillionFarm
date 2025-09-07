using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : BaseFeatureButton
{
    public override void Button_Click()
    {
        base.Button_Click();
        InventoryDialog.DoShowDialog();
    }
}
