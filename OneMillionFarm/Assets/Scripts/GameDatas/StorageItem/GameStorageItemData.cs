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

    public void AddAmount(int amount) 
    {
        TypeAmount.AddAmount(amount);
    }
}
