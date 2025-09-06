using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameCreatureDatas : BaseGameData
{
    public static GameCreatureDatas Instance
    {
        get
        {
            var gameManager = GameDataManager.Instance;
            if (gameManager == null)
                return null;

            return gameManager.CreatureDatas;
        }
    }

    public List<GameCreatureData> creatureDatas;

    public List<GameCreatureData> CreatureDatas
    {
        get
        {
            if (creatureDatas == null)
            {
                Init();
            }

            return creatureDatas;
        }
    }

    public List<GameCreatureData> GetCloneCreatureDatas()
    {
        var results = new List<GameCreatureData>();
        if (CreatureDatas == null)
            return results;

        for (int i = 0; i < creatureDatas.Count; i++)
        {
            results.Add(creatureDatas[i].Clone());
        }
        return results;
    }

    public override void Init()
    {
        base.Init();
        creatureDatas = new List<GameCreatureData>();
    }
}
