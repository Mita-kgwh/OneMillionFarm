using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/CreatureStatsConfigs", fileName = "CreatureStatsConfigs")]
public class CreatureStatsConfigs : ScriptableObject
{
    public static CreatureStatsConfigs Instance
    {
        get
        {
            return CreaturesManager.Instance.CreatureStatsConfigs;
        }
    }

    public List<CreatureStatsConfig> StatsConfigs;
    public float timeCreatureRottenBySec = 3600f;
}

[System.Serializable]
public class CreatureStatsConfig
{
    public ItemType creatureType;
    public float cycleTimeBySec;
    public int cycleLifeCount;
}
