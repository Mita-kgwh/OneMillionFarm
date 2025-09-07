using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkableObject : BaseObject
{
    public int workerID;

    public bool CanAssignWorker => this.workerID <= 0;

    public virtual void AssignWorker(int workerID)
    {
        this.workerID = workerID;
    }

    public virtual void UnassignWorker()
    {
        this.workerID = -1;
    }

    public virtual void WorkerDoInteractAction()
    {

    }
}
