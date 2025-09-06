using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotItemLayout : MonoBehaviour
{
    [SerializeField] protected SlotItemUI slotPf;
    [SerializeField] protected Transform slotsContain;
    [SerializeField] protected List<SlotItemUI> slots = new List<SlotItemUI>();
    [SerializeField] protected int initSlotCount = 7;

    public int InitSlotCount => this.initSlotCount;

    private Dictionary<int, SlotItemUI> dicFindSlots = new Dictionary<int, SlotItemUI>();

    public void InitLayout(SlotType slotType)
    {
        if (slotPf == null)
        {
            Debug.LogError("Slot Pf null");
            return;
        }

        if (slotsContain == null)
            slotsContain = this.transform;

        while (slots.Count < initSlotCount)
        {
            slots.Add(Instantiate(slotPf, slotsContain));
        }

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetIndex(i, slotType)
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
        if (itemDatas == null)
        {
            Debug.LogError("Cannot parse layout because data null");
            return;
        }
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetEmpty();
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
