using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : BaseObject
{
    public int FarmID => this.objectID;

    public static System.Action<FarmTile> OnFarmTileFree;

    /// <summary>
    /// Worker which working on this tile
    /// </summary>
    private WorkerActor workerActor;

    /// <summary>
    /// Plant or Animal on Tile
    /// </summary>
    private BaseCreatureItem creatureItem;

    public bool IsFree => creatureItem == null;

    /// <summary>
    /// Parse worker id when create worker
    /// </summary>
    /// <param name="_farmID"></param>
    /// <returns></returns>
    public FarmTile SetUpFarmTile(int _farmID)
    {
        this.objectID = _farmID;
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
        var slotItem = SlotsClickManager.Instance.CurrentSlotItemUI;
        if (slotItem == null)
        {
            return;
        }

        bool useSuccess = GameStorageItemDatas.Instance.UseStorageItemData(slotItem.SlotIndex);
        if (useSuccess)
        {
            var createData = new GameCreatureData(ConvertItemCreature(slotItem.ItemType), this.objectID);
            createData.SetStartTime(GameUltis.GetLocalLongTime());
            var createObj = CreaturesManager.Instance.CreateCreatureItem(createData);
            if (createObj != null)
            {
                this.creatureItem = createObj;
                this.creatureItem.transform.position = this.transform.position;
            }        
        }
        OnInteractAction?.Invoke(slotItem.ItemType, useSuccess);

        ItemType ConvertItemCreature(ItemType seedType)
        {
            return (ItemType)((int)seedType + 100); 
        }
    }
}
