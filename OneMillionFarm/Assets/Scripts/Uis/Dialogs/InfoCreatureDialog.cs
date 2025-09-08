using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoCreatureDialog : BaseDialog
{
    public TMPro.TextMeshProUGUI tmpName;
    public TMPro.TextMeshProUGUI tmpCurrentProduct;    
    public TMPro.TextMeshProUGUI tmpCollectedProduct;    
    public TMPro.TextMeshProUGUI tmpCycleLeft;    
    public TMPro.TextMeshProUGUI tmpCycleTime;    
    public TMPro.TextMeshProUGUI tmpTimer;
    public Slider sliderCycleLeft;
    public Slider sliderTimerLeft;
    private BaseCreatureItem creatureItem;
    private float timer, fullTimer;

    public static InfoCreatureDialog DoShowDialog(BaseCreatureItem creatureItem)
    {
        var dialog = DialogManager.Instance.GetDialog(DialogType.INFO_CREATURE_DIALOG);
        if (dialog == null)
            return null;
        if (dialog is InfoCreatureDialog inventoryDialog)
        {
            inventoryDialog.ParseData(creatureItem);
            inventoryDialog.ShowDialog();

            return inventoryDialog;
        }

        return null;
    }

    private void Update()
    {
        if (timer < 0)
        {
            ParseData(creatureItem);
            return;
        }

        timer -= Time.deltaTime;
        this.tmpTimer.SetText($"{GameUltis.ConvertFloatToTimeLargestTwoUnit(timer)}");
        this.sliderTimerLeft.value = this.timer / fullTimer;
    }

    public void ParseData(BaseCreatureItem _creatureItem)
    {
        this.creatureItem = _creatureItem;
        if (creatureItem == null)
        {
            return;
        }
        this.timer = creatureItem.timer;
        this.fullTimer = creatureItem.LifeCycle;
        var currentProduct = creatureItem.CurrentProductAmount;
        var collectedProduct = creatureItem.CollectedProductAmount;
        var maxLifeCycle = creatureItem.MaxLifeCycle;
        this.tmpName.SetText(creatureItem.ObjectType.ToString().Remove(0, 9));
        this.tmpTimer.SetText($"{GameUltis.ConvertFloatToTimeLargestTwoUnit(timer)}");
        this.tmpCycleTime.SetText($"{GameUltis.ConvertFloatToTimeLargestTwoUnit(fullTimer)}");
        this.tmpCurrentProduct.SetText($"Current Product: {currentProduct}");
        this.tmpCollectedProduct.SetText($"Collected Product: {collectedProduct}");
        this.tmpCycleLeft.SetText($"{currentProduct + collectedProduct}/{maxLifeCycle}");
        this.sliderCycleLeft.value = 1 - (float)(currentProduct + collectedProduct) / maxLifeCycle;
        this.sliderTimerLeft.value = this.timer / fullTimer;
    }
}
