using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsClickManager : MonoSingleton<SlotsClickManager>
{
    private SlotItemUI currentSlotItemUI;

    public SlotItemUI CurrentSlotItemUI => this.currentSlotItemUI;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnassignCallback();
    }

    public override void Init()
    {
        base.Init();
        UnassignCallback();
        AssignCallback();
    }

    private void AssignCallback()
    {
        BaseDialog.OnHideDialog += OnHideDialogCallback;
        SlotItemUI.OnClickASlotItemUI += OnClickASlotCallback;
        //BaseObject.OnInteractAction += OnInteractActionCallback;
    }

    private void UnassignCallback()
    {
        BaseDialog.OnHideDialog -= OnHideDialogCallback;
        SlotItemUI.OnClickASlotItemUI -= OnClickASlotCallback;
        //BaseObject.OnInteractAction -= OnInteractActionCallback;
    }

    #region Callback

    //private void OnInteractActionCallback(bool _success)
    //{
    //    if (_success)
    //    {
    //        currentSlotItemUI.HighLight(false);
    //        currentSlotItemUI = null;
    //    }        
    //}

    private void OnHideDialogCallback(BaseDialog dialog)
    {
        if (currentSlotItemUI != null)
        {
            currentSlotItemUI.HighLight(false);
            currentSlotItemUI = null;
        }
    }

    private void OnClickASlotCallback(SlotItemUI slotItem)
    {
        if (slotItem == null)
        {
            return;
        }

        if (slotItem.SlotType != SlotType.TOOLBAR)
        {
            return;
        }

        //If currentSlot == null => Not select any item
        if (currentSlotItemUI == null)
        {
            currentSlotItemUI = slotItem;
            currentSlotItemUI.HighLight(true);
        }
        else
        {
            //currentSlot != null => Have select some item
            //Do switch place
            if (currentSlotItemUI.SlotIndex != slotItem.SlotIndex)
            {
                GameStorageItemDatas.Instance.SwitchStorageItemData(currentSlotItemUI.SlotIndex, slotItem.SlotIndex);
            }
            currentSlotItemUI.HighLight(false);
            slotItem.HighLight(false);
            currentSlotItemUI = null;
        }
    }

    #endregion
}
