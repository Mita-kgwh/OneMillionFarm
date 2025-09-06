using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItemUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] Image highLightImage;

    private int slotIndex;
    private SlotType slotType;

    public static System.Action<SlotType, int> OnClickASlotItemUI;

    public int SlotIndex => this.slotIndex;

    public SlotItemUI SetIndex(int index)
    {
        slotIndex = index;
        return this;
    }

    // Start is called before the first frame update
    public virtual void SetItem(GameStorageItemData itemData)
    {
        //itemIcon.sprite = slot.item.icon;
        itemIcon.color = new Color(1, 1, 1, 1);

        quantityText.gameObject.SetActive(true);
        quantityText.text = itemData.Amount.ToString();
    }

    public virtual void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickASlotItemUI?.Invoke(this.slotType, this.slotIndex);
    }

    public void HighLight(bool highlight)
    {
        highLightImage.gameObject.SetActive(highlight);
    }
}
