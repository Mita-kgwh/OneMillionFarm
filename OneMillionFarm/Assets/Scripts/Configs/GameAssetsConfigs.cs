using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/GameAssetsConfigs", fileName = "GameAssetsConfigs")]
public class GameAssetsConfigs : ScriptableObject
{
    public static GameAssetsConfigs Instance
    {
        get
        {
            return GameDataManager.Instance.AssetsConfigs;
        }
    }

    public List<GameAssetsConfig> assetsConfigs = new List<GameAssetsConfig>();

    public GameAssetsConfig GetGameAssetsConfig(ItemType type)
    {
        for (int i = 0; i < assetsConfigs.Count; i++)
        {
            if (assetsConfigs[i].itemType == type)
            {
                return assetsConfigs[i];
            }
        }

        return null;
    }
}

[System.Serializable]
public class GameAssetsConfig
{
    public ItemType itemType;
    public Sprite iconSpr;
}
