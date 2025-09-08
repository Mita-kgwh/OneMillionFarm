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
        GameDataManager.OnRestartGame += OnRestartGameCallback;
    }

    private void UnassignCallback()
    {
        GameDataManager.OnLoadDataDone -= OnLoadDataDoneCallback;
        GameDataManager.OnRestartGame -= OnRestartGameCallback;
    }

    #region Callback

    private void OnLoadDataDoneCallback()
    {
        StartGame();
    }

    private void OnRestartGameCallback()
    {
        RestartGame();
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

    private void RestartGame()
    {
        //Restart Farm Tile
        FarmTileManager.Instance.RestartGame();

        //Restart Plant/Cow
        CreaturesManager.Instance.RestartGame();

        //Restart Worker
        WorkerManager.Instance.RestartGame();

        StartGame();
    }
}
