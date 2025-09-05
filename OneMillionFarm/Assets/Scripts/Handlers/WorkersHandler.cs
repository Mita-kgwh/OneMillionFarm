using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkersHandler : MonoSingleton<WorkersHandler>
{

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnassignCallback();
    }

    public override void Init()
    {
        base.Init();
        UnassignCallback();
        AssignCallback();
    }

    public void AssignCallback()
    {
        WorkerManager.OnCreateAWorker += OnCreateAWorkerCallback;
    }

    public void UnassignCallback()
    {
        WorkerManager.OnCreateAWorker -= OnCreateAWorkerCallback;
    }

    #region Callback

    private void OnCreateAWorkerCallback(WorkerActor _worker)
    {

    }

    private void OnWorkerCompleteJob(WorkerActor _worker)
    {

    }

    #endregion
}
