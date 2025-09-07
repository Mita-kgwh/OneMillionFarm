using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDialog : BaseDialog
{
    [SerializeField] private StoreItem storeItemPf;
    [SerializeField] private Transform itemContain;
    [SerializeField] private List<StoreItem> storeItems;
    private StoreItemConfigs storeItemConfigs;

    private StoreItemConfigs ItemConfigs
    {
        get
        {
            if (storeItemConfigs == null)
            {
                storeItemConfigs = StoreItemConfigs.Instance;
            }
            return storeItemConfigs;
        }
    }
    private bool inited = false;

    public static StoreDialog DoShowDialog()
    {
        var dialog = DialogManager.Instance.GetDialog(DialogType.STORE_DIALOG);
        if (dialog == null)
            return null;
        if (dialog is StoreDialog storeDialog)
        {
            storeDialog.ParseConfig();
            storeDialog.ShowDialog();

            return storeDialog;
        }

        return null;
    }

    public void ParseConfig()
    {
        if (inited)
        {
            return;
        }

        if (ItemConfigs == null)
        {
            return;
        }
        var itemCfs = storeItemConfigs.ItemConfigs;
        for (int i = 0; i < itemCfs.Count; i++)
        {
            StoreItem item = null;
            if (i >= storeItems.Count)
            {
                item = Instantiate(storeItemPf, itemContain);
                storeItems.Add(item);
            }
            else
            {
                item = storeItems[i];
            }

            item.InitConfig(itemCfs[i]);
        }

        for (int i = itemCfs.Count; i < storeItems.Count; i++)
        {
            storeItems[i].gameObject.SetActive(false);
        }
    }
}
