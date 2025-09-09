using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingDialog : BaseDialog
{
    [SerializeField] private TMPro.TMP_InputField iptLevel;

    public static SettingDialog DoShowDialog()
    {
        var dialog = DialogManager.Instance.GetDialog(DialogType.SETTING_DIALOG);
        if (dialog == null)
            return null;
        if (dialog is SettingDialog settingDialog)
        {
            settingDialog.ShowDialog();

            return settingDialog;
        }

        return null;
    }

    public void Button_AddCoin()
    {
        if (iptLevel == null)
        {
            return;
        }

        if (int.TryParse(iptLevel.text, out int coin))
        {
            UserGameStatsData.Instance.AddCoin(coin);
        }
        else
        {
            Debug.LogError("Invalid coin input");
            return;
        }
    }
}
