using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoSingleton<UpdateManager>
{
    private List<IUpdateable> updateableObjs = new List<IUpdateable>();
    private bool endGame = false;

    public override void Init()
    {
        base.Init();
        UnassignCallback();
        AssignCallback();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnassignCallback();
    }

    private void AssignCallback()
    {
        BaseDialog.OnShowDialog += OnShowDialogCallback;
        GameDataManager.OnRestartGame += OnRestartGameCallback;
    }

    private void UnassignCallback()
    {
        BaseDialog.OnShowDialog -= OnShowDialogCallback;
        GameDataManager.OnRestartGame -= OnRestartGameCallback;
    }

    private void OnShowDialogCallback(BaseDialog baseDialog)
    {
        if (baseDialog.DialogType == DialogType.WIN_GAME_DIALOG)
        {
            endGame = true;
        }
    }

    private void OnRestartGameCallback()
    {
        endGame = false;
    }

    public void RegisterUpdateableObject(IUpdateable obj)
    {
        if (updateableObjs.Contains(obj))
        {
            return;
        }
        updateableObjs.Add(obj);
    }

    public void UnregisterUpdateableObject(IUpdateable obj)
    {
        if (!updateableObjs.Contains(obj))
        {
            return;
        }
        updateableObjs.Remove(obj);
    }

    private void Update()
    {
        if (endGame)
        {
            return;
        }
        float dt = Time.deltaTime;
        for (int i = 0; i < updateableObjs.Count; i++)
        {
            updateableObjs[i].OnUpdate(dt);
        }
    }

}
