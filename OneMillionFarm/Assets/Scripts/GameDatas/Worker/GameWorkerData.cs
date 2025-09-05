using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorkerData : BaseGameData
{
    public int workerID;

    public int WorkerID => workerID;

    public int farmTileID;

    public int FarmTileID => farmTileID;

    public bool IsFree 
    {
        get
        {
            return farmTileID <= 0;
        }
    }

    public GameWorkerData() { }

    public GameWorkerData(int workerID) {  this.workerID = workerID; }

}
