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
    private ItemType itemType;

    public static System.Action<SlotItemUI> OnClickASlotItemUI;

    public int SlotIndex => this.slotIndex;
    public SlotType SlotType => this.slotType;
    public ItemType ItemType => this.itemType;

    public SlotItemUI SetIndex(int index, SlotType slotType)
    {
        this.slotIndex = index;
        this.slotType = slotType;
        this.name = $"Slot_{slotIndex}";
        return this;
    }

    // Start is called before the first frame update
    public virtual void SetItem(GameStorageItemData itemData)
    {
        //itemIcon.sprite = slot.item.icon;
        itemIcon.color = new Color(1, 1, 1, 1);

        quantityText.gameObject.SetActive(true);
        quantityText.text = itemData.Amount.ToString();
        this.itemType = itemData.ItemType;
        var cf = GameAssetsConfigs.Instance.GetGameAssetsConfig(this.itemType);
        if (cf != null)
        {
            this.itemIcon.sprite = cf.iconSpr;
        }
        this.itemIcon.gameObject.SetActive(cf != null);
    }

    public virtual void SetEmpty()
    {
        itemIcon.gameObject.SetActive(false);
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickASlotItemUI?.Invoke(this);
    }

    public void HighLight(bool highlight)
    {
        highLightImage.gameObject.SetActive(highlight);
    }
}
