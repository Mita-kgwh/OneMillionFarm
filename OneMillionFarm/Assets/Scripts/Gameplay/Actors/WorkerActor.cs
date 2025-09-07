using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class WorkerActor : BaseObject, IUpdateable
{
    public float timer = 10f;
    public int WorkerID => this.ObjectID;
    public bool IsFree => workableObject == null;

    private WorkableObject workableObject;

    private WorkerManager workerMan;

    private void Awake()
    {
        workerMan = WorkerManager.Instance;
    }

    private void AssignCallback()
    {
        BaseCreatureItem.OnCreatureCollected += OnCreatureCollectedCallback;
        BaseCreatureItem.OnCreatureEndCycle += OnCreatureEndCycleCallback;
        OnInteractAction += OnCreatureInteractCallback;
        CreaturesManager.OnCreateACreature += OnCreateACreatureCallback;

        UpdateManager.Instance.RegisterUpdateableObject(this);
    }

    private void UnassignCallback()
    {
        BaseCreatureItem.OnCreatureCollected -= OnCreatureCollectedCallback;
        BaseCreatureItem.OnCreatureEndCycle -= OnCreatureEndCycleCallback;
        OnInteractAction -= OnCreatureInteractCallback;
        CreaturesManager.OnCreateACreature -= OnCreateACreatureCallback;

        var updateIns = UpdateManager.Instance;
        if (updateIns != null)
        {
            updateIns.UnregisterUpdateableObject(this);
        }
    }

    private void OnDisable()
    {
        UnassignCallback();
    }

    #region Callback
    /// <summary>
    /// WHen use clo
    /// </summary>
    /// <param name="creatureItem"></param>
    private void OnCreatureCollectedCallback(BaseCreatureItem creatureItem)
    {
        if (IsFree)
        {
            return;
        }

        //Not collected mine
        if (creatureItem != this.workableObject)
        {
            return;
        }

        //Time to find new object
        FindWorkableObject();
    }

    private void OnCreatureEndCycleCallback(BaseCreatureItem creatureItem)
    {
        if (IsFree)
        {
            FindWorkableObject();
        }
    }


    /// <summary>
    /// Call when user click tile or plant or cow
    /// </summary>
    /// <param name="baseObject"></param>
    /// <param name="success"></param>
    private void OnCreatureInteractCallback(BaseObject baseObject, bool success)
    {
        if (IsFree)
        {
            return;
        }

        //User interact with other objects, not mine
        if (baseObject != this.workableObject)
        {
            return;
        }

        //User interact with mine, find new one
        FindWorkableObject();
    }

    private void OnCreateACreatureCallback(BaseCreatureItem creatureItem)
    {
        if (!IsFree)
        {
            return;
        }

        //if not busy, then find new obj
        FindWorkableObject();
    }

    #endregion

    /// <summary>
    /// Parse worker id when create worker
    /// </summary>
    /// <param name="workerId"></param>
    /// <returns></returns>
    public WorkerActor SetUpWorker(int workerId)
    {
        this.objectID = workerId;
        UnassignCallback();
        AssignCallback();
        timer = workerMan?.TimeWorkerDoAction ?? 120f;
        DelayFindWorkableObject();
        return this;
    }

    private void DelayFindWorkableObject()
    {
        StartCoroutine(IE_Wait2FindWorkableObject());
    }

    private IEnumerator IE_Wait2FindWorkableObject()
    {
        yield return new WaitForSeconds(0.1f);
        FindWorkableObject();
    }
    
    public override void DoInteractAction()
    {
        base.DoInteractAction();
        Debug.Log($"This is Worker {WorkerID}");
    }

    public void OnUpdate(float dt)
    {
        if (IsFree)
        {
            //Dont have thing to do, stop count time
            return;
        }

        timer -= dt;
        if (timer <= 0)
        {
            timer = workerMan?.TimeWorkerDoAction ?? 120f;
            OnCompleteProgressTask();
        }
    }

    private void OnCompleteProgressTask()
    {
        if (IsFree)
        {
            return;
        }

        //Do action
        workableObject.WorkerDoInteractAction();

        //Find new
        FindWorkableObject();
    }

    /// <summary>
    /// Priority => Collect Creature > Plant/Raise in FarmTile
    /// </summary>
    private void FindWorkableObject()
    {
        if (workableObject != null)
        {
            workableObject.UnassignWorker();
            workableObject = null;
        }

        //Priority => Collect Creature > Plant/Raise in FarmTile
        this.workableObject = CreaturesManager.Instance.GetCanCollectCreatureItem();
        if (!CheckValidObject())
        {
            this.workableObject = FarmTileManager.Instance.GetFreeFarmTile();
            if (CheckValidObject())
            {
                //Nothing to do, wait for new thing happen (free farm tile/have thing to collect)
                return;
            }
        }

        bool CheckValidObject()
        {
            if (this.workableObject == null)
            {
                return false;
            }

            if (workableObject.CanAssignWorker)
            {
                workableObject.AssignWorker(WorkerID);
                return true;
            }

            return false;
        }
    }
}
