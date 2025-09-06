using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoSingleton<DialogManager>
{
    [SerializeField] private List<BaseDialog> baseDialogs = new List<BaseDialog>();

    private Dictionary<DialogType, BaseDialog> dicFindDialog = new Dictionary<DialogType, BaseDialog>();

    public override void Init()
    {
        base.Init();
        for (int i = 0; i < baseDialogs.Count; i++)
        {
            baseDialogs[i].CloseDialog();
        }
    }

    public BaseDialog GetDialog(DialogType dialogType)
    {
        BaseDialog targetDialog = null;
        if (!dicFindDialog.TryGetValue(dialogType, out targetDialog))
        {
            for (int i = 0; i < baseDialogs.Count; i++)
            {
                if (baseDialogs[i].DialogType == dialogType)
                {
                    targetDialog = baseDialogs[i];
                    dicFindDialog.TryAdd(dialogType, baseDialogs[i]);
                    break;
                }
            }
        }

        return targetDialog;
    }
}
