using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/GameWorkersConfigs", fileName = "GameWorkersConfigs")]
public class GameWorkersConfigs : ScriptableObject
{
    public int costBuyWorker = 500;
    public float timeWorkerDoAction = 2f * 60f;

    public int CostBuyWorker => this.costBuyWorker;
    public float TimeWorkerDoAction => this.timeWorkerDoAction;
}
