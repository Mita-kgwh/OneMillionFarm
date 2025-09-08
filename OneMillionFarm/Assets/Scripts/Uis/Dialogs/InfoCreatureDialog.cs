using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCreatureDialog : BaseDialog
{
    public TMPro.TextMeshProUGUI tmpName;
    public TMPro.TextMeshProUGUI tmpCurrentProduct;    

    public static InfoCreatureDialog DoShowDialog(GameCreatureData _creatureData)
    {
        var dialog = DialogManager.Instance.GetDialog(DialogType.INFO_CREATURE_DIALOG);
        if (dialog == null)
            return null;
        if (dialog is InfoCreatureDialog inventoryDialog)
        {
            inventoryDialog.ParseData(_creatureData);
            inventoryDialog.ShowDialog();

            return inventoryDialog;
        }

        return null;
    }

    public void ParseData(GameCreatureData _creatureData)
    {

    }
}
