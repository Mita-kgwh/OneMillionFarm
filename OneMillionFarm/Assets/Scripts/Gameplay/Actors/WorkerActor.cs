using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerActor : MonoBehaviour
{
    public int workerId;

    public int WorkerId => this.workerId;

    private FarmTile workingFarmTile;
    
    /// <summary>
    /// Parse worker id when create worker
    /// </summary>
    /// <param name="workerId"></param>
    /// <returns></returns>
    public WorkerActor SetUpWorker(int workerId)
    {
        this.workerId = workerId;
        return this;
    }

    /// <summary>
    /// Assign farm tile for worker
    /// </summary>
    /// <param name="_farmTile"></param>
    /// <returns></returns>
    public WorkerActor AssignFarmTile(FarmTile _farmTile)
    {
        this.workingFarmTile = _farmTile;
        if (this.workingFarmTile != null)
        {
            this.workingFarmTile.AssignWorker(this);
        }
        return this;
    }

    public WorkerActor UnassignFarmTile()
    {
        if (this.workingFarmTile == null)
        {
            return this;
        }

        this.workingFarmTile.UnassignWorker();
        this.workingFarmTile = null;

        return this;
    }
}
