using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
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
        GameDataManager.OnLoadDataDone += OnLoadDataDoneCallback;
    }

    private void UnassignCallback()
    {
        GameDataManager.OnLoadDataDone -= OnLoadDataDoneCallback;
    }

    #region Callback

    private void OnLoadDataDoneCallback()
    {
        StartGame();
    }

    #endregion

    private void StartGame()
    {
        //Create Farm Tile
        FarmTileManager.Instance.StartGame();

        //Create Plant/Cow
        CreaturesManager.Instance.StartGame();

        //Create Worker
        WorkerManager.Instance.StartGame();
    }
}
