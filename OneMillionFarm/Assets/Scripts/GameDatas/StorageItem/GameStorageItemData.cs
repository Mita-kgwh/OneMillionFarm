using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStorageItemData : BaseGameData
{
    public int slotId;

    public ItemTypeAmount typeAmount;

    public ItemTypeAmount TypeAmount
    {
        get
        {
            if (typeAmount == null)
            {
                typeAmount = new ItemTypeAmount();
            }
            return typeAmount;
        }
    }

    public int Amount
    {
        get
        {
            return TypeAmount.Amount;
        }
    }

    public ItemType ItemType
    {
        get 
        { 
            return TypeAmount.ItemType; 
        }
    }

    public int SlotID => this.slotId;
    public bool FreeSlot => this.ItemType == ItemType.NONE;
    public bool CanUseOnFarmTile => ((int)this.ItemType / 100) == 1;

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

    public GameStorageItemData() 
    {
        
    }

    public GameStorageItemData(int slotId, ItemTypeAmount typeAmount)
    {
        this.slotId = slotId;
        this.typeAmount = typeAmount;
    }

    public GameStorageItemData Clone() 
    {  
        var clone = new GameStorageItemData();
        clone.typeAmount = TypeAmount.Clone();
        clone.slotId = this.slotId;
        return clone;
    }

    public GameStorageItemData SetSlotID(int slotId)
    {
        this.slotId = slotId;
        return this;
    }

    public GameStorageItemData SetStorageItem(ItemType _type, int _amount)
    {
        if (this.typeAmount == null)
        {
            this.typeAmount = new ItemTypeAmount(_type, _amount);
            return this;
        }

        this.typeAmount.SetType(_type, _amount);
        return this;
    }

    public GameStorageItemData SetStorageItem(ItemTypeAmount _typeAmount)
    {
        this.typeAmount = _typeAmount;
        return this;
    }

    public GameStorageItemData AddAmount(int amount) 
    {
        TypeAmount.AddAmount(amount);
        return this;
    }

    public void SellProduct()
    {
        if (TradingMan.DoTrading(ItemType, Amount))
        {
            TypeAmount.Clear();
        }
    }
}
