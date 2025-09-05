using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/GameObjectConfigs", fileName = "GameObjectConfigs")]
public class GameObjectConfigs : ScriptableObject
{
    public List<GameObjectConfig> objConfigs;

    private Dictionary<ItemType, GameObjectConfig> dicFindObjConfig = new Dictionary<ItemType, GameObjectConfig>();
    
    public GameObject GetObjectByType(ItemType _type)
    {
        GameObjectConfig targetObj = null;
        if (!dicFindObjConfig.TryGetValue(_type, out targetObj))
        {
            for (int i = 0; i < objConfigs.Count; i++)
            {
                if (objConfigs[i].objType == _type)
                {
                    targetObj = objConfigs[i];
                    dicFindObjConfig.TryAdd(_type, objConfigs[i]);
                    break;
                }
            }
        }
        return targetObj.gameObj;
    }
}

[System.Serializable]
public class GameObjectConfig
{
    public ItemType objType;
    public GameObject gameObj;
}
