using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : WorkableObject
{
    public int FarmID => this.ObjectID;

    public static System.Action<FarmTile> OnFarmTileFree;

    /// <summary>
    /// Plant or Animal on Tile
    /// </summary>
    private BaseCreatureItem creatureItem;

    public bool IsFree => creatureItem == null;

    protected override void Awake()
    {
        UnassignCallback();
        AssignCallback();
    }

    private void OnDestroy()
    {
        UnassignCallback();
    }

    protected virtual void AssignCallback()
    {
        BaseCreatureItem.OnCreatureReturn2Pool += OnReturn2PoolCallback;
    }

    protected virtual void UnassignCallback()
    {
        BaseCreatureItem.OnCreatureReturn2Pool -= OnReturn2PoolCallback;
    }

    protected virtual void OnReturn2PoolCallback(BaseCreatureItem creatureItem)
    {
        if (this.creatureItem == null)
        {
            return;
        }
        if (this.creatureItem.CreatureID != creatureItem.CreatureID)
        {
            return;
        }
        UnassignCreatureItem();
    }

    /// <summary>
    /// Parse worker id when create worker
    /// </summary>
    /// <param name="_farmID"></param>
    /// <returns></returns>
    public FarmTile SetUpFarmTile(GameFarmTileData _tileData)
    {
        //this.tileData = _tileData;
        //if (this.tileData == null)
        //    return this;
        this.objectID = _tileData.FarmTileID;
        return this;
    }


    public FarmTile AssignCreatureItem(BaseCreatureItem creatureItem)
    {
        this.creatureItem = creatureItem;
        return this;
    }

    public FarmTile UnassignCreatureItem()
    {
        this.creatureItem = null;
        return this;
    }

    public override void DoInteractAction()
    {
        base.DoInteractAction();
        Debug.Log($"This is Farm Tile {FarmID}");
        if (!IsFree)
        {
            return;
        }

        var slotItem = SlotsClickManager.Instance.CurrentSlotItemUI;
        if (slotItem == null)
        {
            return;
        }

        UseStorageItem(slotItem.SlotIndex, slotItem.ItemType);
    }

    private void UseStorageItem(int slotIndex, ItemType itemType)
    {
        bool useSuccess = GameStorageItemDatas.Instance.UseStorageItemData(slotIndex);
        if (useSuccess)
        {
            var createData = GameCreatureDatas.Instance?.OnCreateACreature(GameUltis.ConvertTypeSeed2Creature(itemType), this.objectID) ?? null;
            CreaturesManager.Instance.CreateCreatureItem(createData);
        }
        OnInteractAction?.Invoke(this, useSuccess);
    }

    public override void WorkerDoInteractAction()
    {
        base.WorkerDoInteractAction();
        if (!IsFree)
        {
            return;
        }

        var slotData = GameStorageItemDatas.Instance.GetRandomUsableItemData();
        if (slotData == null)
        {
            Debug.LogError("Have nothing can use in bag");
            return;
        }

        UseStorageItem(slotData.SlotID, slotData.ItemType);
    }
}
