using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : BaseObject
{
    public int FarmID => this.objectID;

    public static System.Action<FarmTile> OnFarmTileFree;

    //protected GameFarmTileData tileData;

    //protected GameFarmTileData TileData
    //{
    //    get
    //    {
    //        if (this.tileData == null)
    //        {

    //        }

    //        return this.tileData;
    //    }
    //}

    /// <summary>
    /// Worker which working on this tile
    /// </summary>
    private WorkerActor workerActor;

    /// <summary>
    /// Plant or Animal on Tile
    /// </summary>
    private BaseCreatureItem creatureItem;

    public bool IsFree => creatureItem == null;

    private void Awake()
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
        BaseCreatureItem.OnReturn2Pool += OnReturn2PoolCallback;
    }

    protected virtual void UnassignCallback()
    {
        BaseCreatureItem.OnReturn2Pool -= OnReturn2PoolCallback;
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

    /// <summary>
    /// Assign worker for farm tile. Call by Worker
    /// </summary>
    /// <param name="_worker"></param>
    /// <returns></returns>
    public FarmTile AssignWorker(WorkerActor _worker)
    {
        this.workerActor = _worker;
        return this;
    }

    /// <summary>
    /// Unassign worker. Call by Worker
    /// </summary>
    /// <returns></returns>
    public FarmTile UnassignWorker()
    {
        this.workerActor = null;
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
        Debug.Log($"This is Farm Tile {objectID}");
        if (!IsFree)
        {
            return;
        }

        var slotItem = SlotsClickManager.Instance.CurrentSlotItemUI;
        if (slotItem == null)
        {
            return;
        }

        bool useSuccess = GameStorageItemDatas.Instance.UseStorageItemData(slotItem.SlotIndex);
        if (useSuccess)
        {
            var createData = GameCreatureDatas.Instance?.OnCreateACreature(ConvertItemCreature(slotItem.ItemType), this.objectID) ?? null;
            var createObj = CreaturesManager.Instance.CreateCreatureItem(createData);
            if (createObj != null)
            {
                this.creatureItem = createObj;
                this.creatureItem.transform.position = this.transform.position;
                AssignCreatureItem(createObj);
            }        
        }
        OnInteractAction?.Invoke(slotItem.ItemType, useSuccess);

        ItemType ConvertItemCreature(ItemType seedType)
        {
            return (ItemType)((int)seedType + 100); 
        }
    }
}
