using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDialog : BaseDialog
{
    [SerializeField] protected SlotItemUI slotPf;
    [SerializeField] protected Transform slotsContain;
    [SerializeField] protected List<SlotItemUI> slots = new List<SlotItemUI>();
    [SerializeField] protected int initSlotCount = 7;

    private Dictionary<int, SlotItemUI> dicFindSlots = new Dictionary<int, SlotItemUI>();
    private bool inited = false;

    public static InventoryDialog DoShowDialog()
    {
        var dialog = DialogManager.Instance.GetDialog(DialogType.INVENTORY_DIALOG);
        if (dialog == null)
            return null;
        if (dialog is InventoryDialog inventoryDialog)
        {
            var itemDatas = GameStorageItemDatas.Instance.GetCloneStorageItemDatas();
            inventoryDialog.ParseData(itemDatas);
            inventoryDialog.ShowDialog();

            return inventoryDialog;
        }

        return null;
    }

    public void InitDialog()
    {
        if (slotPf == null) 
        {
            Debug.LogError("Slot Pf null");
            return;
        }

        if (slotsContain == null)
            slotsContain = this.transform;

        while (slots.Count <= initSlotCount)
        {
            slots.Add(Instantiate(slotPf, slotsContain));
        }

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetIndex(i)
                    .SetEmpty();
            dicFindSlots.TryAdd(i, slots[i]);
        }

        if (slots.Count > initSlotCount)
        {
            //Deactive redundant slot
            for (int i = initSlotCount; i < slots.Count; i++)
            {
                slots[i].gameObject.SetActive(false);
            }
        }
       
    }

    public void ParseData(List<GameStorageItemData> itemDatas)
    {
        if (!inited)
        {
            InitDialog();
            inited = true;
        }

        if (itemDatas == null)
        {
            return;
        }

        for (int i = 0; i < itemDatas.Count; i++)
        {
            if (dicFindSlots.TryGetValue(itemDatas[i].SlotID, out var slotItem))
            {
                slotItem.SetItem(itemDatas[i]);
            }
        }
    }
}
