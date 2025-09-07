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

    public override void OpenGame()
    {
        base.OpenGame();
        //TODO Calculate offline
    }

    public GameCreatureData OnCreateACreature(ItemType creatureType, int farmID)
    {
        var neData = new GameCreatureData(creatureType, farmID);
        CreatureDatas.Add(neData);        
        SaveData();
        return neData;
    }

    /// <summary>
    ///  Return true if be rotton, false if not n add more product.
    ///  If data null, return true to destroy creature(return it to pool)
    /// </summary>
    /// <param name="farmId"></param>
    /// <returns></returns>
    public bool OnACreatureEndCycleDo(int farmId)
    {
        var data = GetCreatureDataByFarmId(farmId);
        if (data != null)
        {
            bool rotton = data.OnCreatureEndCycleDo();
            //Remove data if rotton
            if (rotton)
            {
                CreatureDatas.Remove(data);
            }
            SaveData();
            return rotton;
        }

        return true;
    }

    /// <summary>
    ///  Return true if collect when reach max life time, false if not.
    ///  If data null, return true to destroy creature(return it to pool)
    /// </summary>
    /// <param name="farmId"></param>
    /// <returns></returns>
    public bool OnCollectACreatureProductDo(int farmId)
    {
        var data = GetCreatureDataByFarmId(farmId);
        if (data != null)
        {
            data.OnCollectProductDo();
            bool maxLifeTime = data.ReachMaxLifeTime;
            if (maxLifeTime)
            {
                CreatureDatas.Remove(data);
            }
            SaveData();
            return maxLifeTime;
        }

        return true;
    }

    private GameCreatureData GetCreatureDataByFarmId(int farmId)
    {
        for (int i = 0; i < creatureDatas.Count; i++)
        {
            if (creatureDatas[i].FarmID == farmId)
            {
                return creatureDatas[i];
            }
        }

        return null;
    }
}
