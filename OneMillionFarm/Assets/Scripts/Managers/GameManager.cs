using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        //Create Farm Tile

        //Create Plant/Cow

        //Create Worker
        WorkerManager.Instance.StartGame();
    }
}
