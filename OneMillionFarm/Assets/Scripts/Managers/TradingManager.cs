using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradingManager : MonoSingleton<TradingManager>
{
    [SerializeField] private StoreItemConfigs storeItemConfigs;

    public static Action<bool, ItemType, int> OnTradingSuccess;

    public StoreItemConfigs ItemConfigs => storeItemConfigs;

    private UserGameStatsData userGameStatsData;

    private UserGameStatsData UserGameStatsData
    {
        get
        {
            if (userGameStatsData == null)
            {
                userGameStatsData = UserGameStatsData.Instance;
            }
            return userGameStatsData;
        }
    }

    private GameStorageItemDatas storageItemDatas;

    private GameStorageItemDatas StorageItemDatas
    {
        get
        {
            if (storageItemDatas == null)
            {
                storageItemDatas = GameStorageItemDatas.Instance;
            }
            return storageItemDatas;
        }
    }

    public bool IsCanPurchase(int tradingValue)
    {
        if (UserGameStatsData == null)
        {
            return false;
        }

        return userGameStatsData.IsCanUse(tradingValue);
    }

    public bool DoTrading(ItemType itemType, int tradingAmount)
    {
        if (storeItemConfigs == null)
        {
            return false;
        }

        var tradingCf = GetTradingConfigByType(itemType);

        if (tradingCf == null)
        {
            return false;
        }

        int category = (int)itemType / 100;
        int tradingValue = tradingCf.TradingValue;
        switch (category)
        {
            case 1:
                return PurchaseItem(itemType, tradingValue, tradingAmount);
            case 3:
                return SellItem(itemType, tradingValue, tradingAmount);
            default:
                return false;
        }
        
    }

    private bool PurchaseItem(ItemType itemType, int tradingValue, int tradingAmount)
    {
        if (UserGameStatsData == null || StorageItemDatas == null)
        {
            return false;
        }

        if (!userGameStatsData.IsCanUse(tradingValue) || storageItemDatas.IsFull)
        {
            return false;
        }

        userGameStatsData.UseCoin(tradingValue);
        OnTradingSuccess?.Invoke(true, itemType, tradingAmount);

        return true;
    }

    private bool SellItem(ItemType itemType, int tradingValue, int tradingAmount)
    {
        if (UserGameStatsData == null)
        {
            return false;
        }

        userGameStatsData.AddCoin(tradingValue);
        OnTradingSuccess?.Invoke(false, itemType, tradingAmount);

        return true;
    }

    private StoreItemConfig GetTradingConfigByType(ItemType _type)
    {
        if (storeItemConfigs == null)
        {
            return null;
        }

        var itemCfs = storeItemConfigs.ItemConfigs;
        for (int i = 0; i < itemCfs.Count; i++)
        {
            if (itemCfs[i].TypeItem == _type)
            {
                return itemCfs[i];
            }
        }

        return null;
    }
}
