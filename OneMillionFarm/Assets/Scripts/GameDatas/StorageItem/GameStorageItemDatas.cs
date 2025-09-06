using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStorageItemDatas : BaseGameData
{
    public static GameStorageItemDatas Instance
    {
        get
        {
            var gameManager = GameDataManager.Instance;
            if (gameManager == null)
                return null;

            return gameManager.StorageItemDatas;
        }
    }

    public static System.Action OnStorageDataChange;

    public List<GameStorageItemData> storageItemDatas;

    public List<GameStorageItemData> StorageItemDatas
    {
        get
        {
            if (storageItemDatas == null)
            {
                Init();
            }
            return storageItemDatas;
        }
    }

    private Dictionary<int, GameStorageItemData> dicFindStorageItem = new Dictionary<int, GameStorageItemData>();

    public List<GameStorageItemData> GetCloneStorageItemDatas()
    {
        var results = new List<GameStorageItemData>();
        if (StorageItemDatas == null)
            return results;

        for (int i = 0; i < storageItemDatas.Count; i++)
        {
            results.Add(storageItemDatas[i].Clone());
        }
        return results;
    }

    public override void Init()
    {
        base.Init();
        storageItemDatas = new List<GameStorageItemData>();
        dicFindStorageItem = new Dictionary<int, GameStorageItemData>();
        var statsCf = GameStatsConfigs.Instance;
        if (statsCf == null)
        {
            return;
        }
        var startItemAmount = statsCf.ItemAmountClone();
        for (int i = 0; i < startItemAmount.Count; i++)
        {
            AddStorageItemData(startItemAmount[i], i);
        }
    }

    public bool AddStorageItemData(ItemTypeAmount typeAmount, int slotId)
    {
        var itemData = GetGameStorageItemData(slotId);
        if (itemData == null)
        {
            itemData = new GameStorageItemData(slotId, typeAmount);
            storageItemDatas.Add(itemData);
            dicFindStorageItem.TryAdd(slotId, itemData);

            return true;
        }
        else
        {
            Debug.LogError("This slot has data already, check again");
        }

        return false;
    }

    public bool UseStorageItemData(int slotId, int amount = 1)
    {
        if (amount <= 0)
        {
            return false;
        }
        var itemData = GetGameStorageItemData(slotId);
        if (itemData == null)
        {
            Debug.LogError("This slot data null, can not use");
            return false;
        }

        itemData.AddAmount(-amount);

        return true;
    }

    private GameStorageItemData GetGameStorageItemData(int slotId)
    {
        GameStorageItemData itemData = null;
        if (!dicFindStorageItem.TryGetValue(slotId, out itemData))
        {
            for (int i = 0; i < storageItemDatas.Count; i++)
            {
                if (storageItemDatas[i].SlotID == slotId)
                {
                    itemData = storageItemDatas[i];
                    dicFindStorageItem.TryAdd(slotId, itemData);
                    break;
                }
            }
        }

        return itemData;
    }

    public GameStorageItemData GetCloneGameStorageItemData(int slotId)
    {
        GameStorageItemData cloneItemData = null;
        GameStorageItemData orgItemData = GetGameStorageItemData(slotId);
        if (orgItemData != null)
        {
            cloneItemData = orgItemData.Clone();
        }
        return cloneItemData;
    }

    public void SwitchStorageItemData(int slotID1, int slotID2)
    {
        var data1 = GetGameStorageItemData(slotID1);
        var data2 = GetGameStorageItemData(slotID2);
        dicFindStorageItem.Remove(slotID1);
        dicFindStorageItem.Remove(slotID2);
        if (data1 != null)
        {            
            data1.SetSlotID(slotID2);
            dicFindStorageItem.TryAdd(slotID2, data1);
        }

        if (data2 != null)
        {
            data2.SetSlotID(slotID1);
            dicFindStorageItem.TryAdd(slotID1, data2);
        }

        OnStorageDataChange?.Invoke();
    }
}
