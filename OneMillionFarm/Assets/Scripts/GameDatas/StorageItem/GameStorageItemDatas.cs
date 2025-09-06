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

    public GameStorageItemData GetGameStorageItemData(int slotId)
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
}
