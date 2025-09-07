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
        var storageSize = statsCf.StorageSize;
        for (int i = 0; i < storageSize; i++)
        {
            CreateStorageItemData(new ItemTypeAmount(), i);
        }
        for (int i = 0; i < startItemAmount.Count; i++)
        {
            var slotData = GetGameStorageItemDataEmpty();
            if (slotData != null)
            {
                slotData.SetStorageItem(startItemAmount[i]);
            }
        }

        SaveData();
    }

    public override void OpenGame()
    {
        base.OpenGame();
        UnassignCallback();
        AssignCallback();
    }

    private GameDataManager mainDataInstance;
    protected override void SaveData()
    {
        base.SaveData();
        if (mainDataInstance == null)
        {
            mainDataInstance = GameDataManager.Instance;
        }
        if (mainDataInstance == null)
        {
            Debug.LogError("Data Manager Null, can not save");
            return;
        }
        mainDataInstance.SaveData();
    }

    private void AssignCallback()
    {
        GameCreatureDatas.OnCollectProduct += OnCollectProductCallback;
    }

    private void UnassignCallback()
    {
        GameCreatureDatas.OnCollectProduct -= OnCollectProductCallback;
    }

    private void OnCollectProductCallback(ItemType creatureType, int amount, int farmID)
    {
        var productType = GameUltis.ConvertTypeCreature2Product(creatureType);
        var dataSlot = GetGameStorageItemDataByType(productType);
        if (dataSlot != null)
        {
            dataSlot.AddAmount(amount);
        }
        else
        {
            //Has not have this item type yet
            dataSlot = GetGameStorageItemDataEmpty();
            if (dataSlot == null)
            {
                Debug.LogError("Bag Full");
            }
            else
            {
                dataSlot.SetStorageItem(new ItemTypeAmount(productType, amount));
            }            
        }
        OnStorageDataChange?.Invoke();
        SaveData();
    }

    /// <summary>
    /// Create add item data, remember to call save after
    /// </summary>
    /// <param name="typeAmount"></param>
    /// <param name="slotId"></param>
    /// <returns></returns>
    private bool CreateStorageItemData(ItemTypeAmount typeAmount, int slotId)
    {
        var itemData = new GameStorageItemData(slotId, typeAmount);
        storageItemDatas.Add(itemData);
        dicFindStorageItem.TryAdd(slotId, itemData);
        return true;
    }

    public bool UseStorageItemData(int slotId, int amount = 1)
    {
        if (amount <= 0)
        {
            return false;
        }
        var itemData = GetGameStorageItemDataBySlotId(slotId);
        if (itemData == null)
        {
            Debug.LogError("This slot data null, can not use");
            return false;
        }
        if (!itemData.CanUseOnFarmTile)
        {
            Debug.LogError($"Invalid, can not use {itemData.ItemType} on farm tile.");
            return false;
        }
        itemData.AddAmount(-amount);
        OnStorageDataChange?.Invoke();
        SaveData();
        return true;
    }

    private GameStorageItemData GetGameStorageItemDataEmpty()
    {
        GameStorageItemData itemData = null;

        for (int i = 0; i < storageItemDatas.Count; i++)
        {
            if (storageItemDatas[i].FreeSlot)
            {
                itemData = storageItemDatas[i];
                break;
            }
        }

        return itemData;
    }

    public GameStorageItemData GetGameStorageItemDataBySlotId(int slotId)
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

    public GameStorageItemData GetRandomUsableItemData()
    {
        int startId = (int)ItemType.SEED_TOMATO;
        //TODO TEST
        int endId = (int)ItemType.SEED_BLUE_BERRY;
        List<GameStorageItemData> rdDataItems = new List<GameStorageItemData>();
        for (int i = startId; i < endId; i++)
        {
            var itemData = GetGameStorageItemDataByType((ItemType)i);
            if (itemData != null)
            {
                rdDataItems.Add(itemData);
            }
        }
        if (rdDataItems.Count == 0)
        {
            return null;
        }
        
        return rdDataItems[Random.Range(0, rdDataItems.Count)];
    }

    private GameStorageItemData GetGameStorageItemDataByType(ItemType itemType)
    {
        GameStorageItemData itemData = null;
        for (int i = 0; i < storageItemDatas.Count; i++)
        {
            if (storageItemDatas[i].ItemType == itemType)
            {
                itemData = storageItemDatas[i];
                break;
            }
        }

        return itemData;
    }

    public void SwitchStorageItemData(int slotID1, int slotID2)
    {
        var data1 = GetGameStorageItemDataBySlotId(slotID1);
        var data2 = GetGameStorageItemDataBySlotId(slotID2);
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
        SaveData();
    }
}
