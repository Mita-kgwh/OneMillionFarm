using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDialog : BaseDialog
{
    [SerializeField] protected SlotItemLayout slotItemLayout;
    [SerializeField] protected TMPro.TextMeshProUGUI tmpProductsValue;
    [SerializeField] protected Button btnSell;
    private bool inited = false;

    public static InventoryDialog DoShowDialog()
    {
        var dialog = DialogManager.Instance.GetDialog(DialogType.INVENTORY_DIALOG);
        if (dialog == null)
            return null;
        if (dialog is InventoryDialog inventoryDialog)
        {            
            inventoryDialog.ParseData();
            inventoryDialog.ShowDialog();

            return inventoryDialog;
        }

        return null;
    }

    private void OnEnable()
    {
        UnassignCallback();
        AssignCallback();
    }

    private void OnDisable()
    {
        UnassignCallback();
    }

    private void AssignCallback()
    {
        GameStorageItemDatas.OnStorageDataChange += OnStorageDataChangeCallback;
    }

    private void UnassignCallback()
    {
        GameStorageItemDatas.OnStorageDataChange -= OnStorageDataChangeCallback;
    }

    private void OnStorageDataChangeCallback()
    {
        ParseData();
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

    public void ParseData()
    {
        var itemDatas = GameStorageItemDatas.Instance.StorageItemDatas;

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

        var allProductValue = GameStorageItemDatas.Instance.GetSumValueAllProductsInBag();
        btnSell.gameObject.SetActive(allProductValue > 0);
        tmpProductsValue.SetText($"{allProductValue}");
    }

    public void Button_SellAllProduct()
    {
        GameStorageItemDatas.Instance.SellAllProduct();
    }
}
