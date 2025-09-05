using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTilesHandler : MonoSingleton<FarmTilesHandler>
{
    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnassignCallback();
    }
    public override void Init()
    {
        base.Init();
        UnassignCallback();
        AssignCallback();
    }

    public void AssignCallback()
    {
        
    }

    public void UnassignCallback()
    {
        
    }

    #region Callback

    private void OnCreateAFarmTileCallback(FarmTile _farmTile)
    {

    }

    #endregion
}
