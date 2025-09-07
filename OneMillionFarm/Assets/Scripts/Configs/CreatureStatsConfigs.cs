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

    public float TimeCreatureRottenBySec => timeCreatureRottenBySec;

    public CreatureStatsConfig GetCreatureStatsConfig(ItemType _creatureType)
    {
        for (int i = 0; i < StatsConfigs.Count; i++)
        {
            if (StatsConfigs[i].CreatureType == _creatureType)
            {
                return StatsConfigs[i];
            }
        }

        return null;
    }
}

[System.Serializable]
public class CreatureStatsConfig
{
    public ItemType creatureType;
    public float cycleTimeBySec;
    public int cycleLifeCount;

    public ItemType CreatureType => this.creatureType;
    public float CycleTimeBySec => this.cycleTimeBySec;
    public int CycleLifeCount => this.cycleLifeCount;

}
