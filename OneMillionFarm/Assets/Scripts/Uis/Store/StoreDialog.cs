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
        if (ItemConfigs == null)
        {
            return;
        }

        if (!inited)
        {
            InitUI();
        }

        for (int i = 0; i < storeItems.Count; i++)
        {
            storeItems[i].CheckPurchase();
        }                
    }

    private void InitUI()
    {
        var itemCfs = storeItemConfigs.ItemConfigs;
        int counterItem = 0;
        for (int i = 0; i < itemCfs.Count; i++)
        {
            if ((int)itemCfs[i].TypeItem / 100 != 1)
            {
                continue;
            }
            counterItem++;
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

        for (int i = counterItem; i < storeItems.Count; i++)
        {
            storeItems[i].gameObject.SetActive(false);
        }

        inited = true;
    }
}
