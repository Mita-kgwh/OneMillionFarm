using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBar : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private SlotItemLayout slotItemLayout;
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
        GameDataManager.OnLoadDataDone += OnLoadDataDoneCallback;

        BaseDialog.OnShowDialog += OnShowDialogCallback;
        BaseDialog.OnHideDialog += OnHideDialogCallback;

        GameStorageItemDatas.OnStorageDataChange += OnStorageDataChangeCallback;
    }

    private void UnassignCallback()
    {
        GameDataManager.OnLoadDataDone -= OnLoadDataDoneCallback;

        BaseDialog.OnShowDialog -= OnShowDialogCallback;
        BaseDialog.OnHideDialog -= OnHideDialogCallback;

        GameStorageItemDatas.OnStorageDataChange -= OnStorageDataChangeCallback;
    }
    
    #region Callback
    
    private void OnLoadDataDoneCallback()
    {
        InitLayout();
        ParseData();
    }

    private void OnShowDialogCallback(BaseDialog dialog)
    {
        DialogType dialogType = dialog?.DialogType ?? DialogType.NONE;
        if (dialogType != DialogType.INVENTORY_DIALOG)
        {
            return;
        }
        ShowToolBar(false);
    }

    private void OnHideDialogCallback(BaseDialog dialog)
    {
        DialogType dialogType = dialog?.DialogType ?? DialogType.NONE;
        if (dialogType != DialogType.INVENTORY_DIALOG)
        {
            return;
        }
        ParseData();
        ShowToolBar(true);
    }

    private void OnStorageDataChangeCallback()
    {
        ParseData();
    }
    
    #endregion

    private void ShowToolBar(bool _show)
    {
        this.canvasGroup.alpha = _show ? 1 : 0;
        this.canvasGroup.blocksRaycasts = _show;
        this.canvasGroup.interactable = _show;
    }
    
    private void InitLayout()
    {
        if (slotItemLayout == null)
        {
            Debug.LogError("Slot item layout null");
            return;
        }
        slotItemLayout.InitLayout(SlotType.TOOLBAR);
    }

    private void ParseData()
    {
        if (slotItemLayout == null)
        {
            Debug.LogError("Slot item layout null");
            return;
        }
        var datas = GameStorageItemDatas.Instance;
        if (datas == null)
        {
            Debug.LogError("Storage Data null");
            return;
        }
        var topShowItemDatas = new List<GameStorageItemData>();
        var amount = slotItemLayout.InitSlotCount;
        for (int i = 0; i < amount; i++)
        {
            var data = datas.GetGameStorageItemDataBySlotId(i);
            if (data == null)
            {
                continue;
            }
            topShowItemDatas.Add(data);
        }
        slotItemLayout.ParseData(topShowItemDatas);
    }
}
