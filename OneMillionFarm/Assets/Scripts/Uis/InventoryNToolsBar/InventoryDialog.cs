using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDialog : BaseDialog
{
    [SerializeField] protected SlotItemLayout slotItemLayout;
    private bool inited = false;

    public static InventoryDialog DoShowDialog()
    {
        var dialog = DialogManager.Instance.GetDialog(DialogType.INVENTORY_DIALOG);
        if (dialog == null)
            return null;
        if (dialog is InventoryDialog inventoryDialog)
        {
            var itemDatas = GameStorageItemDatas.Instance.StorageItemDatas;
            inventoryDialog.ParseData(itemDatas);
            inventoryDialog.ShowDialog();

            return inventoryDialog;
        }

        return null;
    }

    private void InitDialog()
    {
        if (slotItemLayout == null)
        {
            Debug.LogError("Slot item layout null");
            return;
        }
        slotItemLayout.InitLayout(SlotType.INVENTORY);
    }

    public void ParseData(List<GameStorageItemData> itemDatas)
    {
        if (slotItemLayout == null)
        {
            Debug.LogError("Slot item layout null");
            return;
        }

        if (!inited)
        {
            InitDialog();
            inited = true;
        }
        
        slotItemLayout.ParseData(itemDatas);        
    }

    public void Button_SellAllProduct()
    {
        GameStorageItemDatas.Instance.SellAllProduct();
    }
}
