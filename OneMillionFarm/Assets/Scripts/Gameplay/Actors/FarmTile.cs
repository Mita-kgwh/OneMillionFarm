using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : MonoBehaviour
{
    public int farmID;

    public int FarmID => this.farmID;

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
        this.farmID = _farmID;
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
}
