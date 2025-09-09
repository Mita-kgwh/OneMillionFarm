using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameDialog : BaseDialog
{
    public static WinGameDialog DoShowDialog()
    {
        DialogManager.Instance.CloseAllDialog(DialogType.WIN_GAME_DIALOG);
        var dialog = DialogManager.Instance.GetDialog(DialogType.WIN_GAME_DIALOG);
        if (dialog == null)
            return null;
        if (dialog is WinGameDialog winGameDialog)
        {
            winGameDialog.ShowDialog();

            return winGameDialog;
        }

        return null;
    }

    protected override void OnCompleteShow()
    {
        base.OnCompleteShow();
        GameDataManager.Instance.CreateNewData();
        PlayerPrefs.DeleteAll();
    }

    public void Button_RestartGame()
    {
        GameDataManager.Instance.ReStartGame();
        CloseDialog();
    }
}
