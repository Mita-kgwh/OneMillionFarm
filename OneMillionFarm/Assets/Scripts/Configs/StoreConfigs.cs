using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/StoreItemConfigs", fileName = "StoreItemConfigs")]
public class StoreItemConfigs : ScriptableObject
{
    public static StoreItemConfigs Instance
    {
        get
        {
            return TradingManager.Instance.ItemConfigs;
        }
    }

    public List<StoreItemConfig> storeItemConfigs;

    public List<StoreItemConfig> ItemConfigs
    {
        get
        {
            if (storeItemConfigs == null)
            {
                storeItemConfigs = new List<StoreItemConfig>();
            }
            return storeItemConfigs;
        }
    }

    public StoreItemConfig GetStoreItemConfigByType(ItemType _itemType)
    {
        for (int i = 0; i < storeItemConfigs.Count; i++)
        {
            if (storeItemConfigs[i].TypeItem == _itemType)
            {
                return storeItemConfigs[i];
            }
        }

        return null;
    }
}

[System.Serializable]
public class StoreItemConfig
{
    public ItemType itemType;
    public string nameItem;
    public int tradingValue;
    public int tradingAmount;

    public ItemType TypeItem => this.itemType;
    public string NameItem => this.nameItem;
    public int TradingValue => this.tradingValue;
    public int TradingAmount => this.tradingAmount;

    private TradingManager tradingMan;
    
    private TradingManager TradingMan
    {
        get
        {
            if (tradingMan == null)
            {
                tradingMan = TradingManager.Instance;
            }

            return tradingMan;
        }
    }

    public bool IsCanPurchase
    {
        get
        {
            return TradingMan.IsCanPurchase(tradingValue);
        }
    }

    public bool IsCanSell => (int)itemType / 100 == 3;

    public bool TradingItem()
    {
        return TradingMan.DoTrading(itemType, tradingAmount);
    }

}


