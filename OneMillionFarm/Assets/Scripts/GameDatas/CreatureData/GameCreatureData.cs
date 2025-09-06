using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameCreatureData : BaseGameData
{
    public GameCreatureData() { }

    public GameCreatureData Clone()
    {
        var clone = new GameCreatureData();

        return clone;
    }
}
