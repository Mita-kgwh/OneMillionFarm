using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTileManager : MonoSingleton<FarmTileManager>
{
    [SerializeField] protected GridObjectLayoutGroup gridLayout3D;
    [SerializeField] protected Transform gridContainTf;
    public List<FarmTile> tiles;

    private GameFarmTileDatas farmTileDatas;

    private GameFarmTileDatas FarmTileDatas
    {
        get
        {
            if (farmTileDatas == null)
            {
                farmTileDatas = GameFarmTileDatas.Instance;
            }

            return farmTileDatas;
        }
    }

    private Dictionary<int, FarmTile> dicFindFarmTile = new Dictionary<int, FarmTile>();

    private int costBuyFarmTile = 500;
    public static System.Action<FarmTile> OnBuyAFarmTile;
    
    public void StartGame()
    {
        if (FarmTileDatas == null)
        {
            Debug.LogError("Farm Tile Datas Null, can not start game");
            return;
        }

        if (gridLayout3D == null)
        {
            Debug.LogError("Grid Layout Null, can not start game");
            return;
        }

        gridLayout3D.ConstraintCount = FarmTileDatas.MaxColumnFarmTile;
        var farmTileDatasClone = FarmTileDatas.GetCloneFarmTileDatas();
        for (int i = 0; i < farmTileDatasClone.Count; i++)
        {
            CreateFarmTile(farmTileDatasClone[i]);
        }

        // Update 3D layout after creating all blocks
        gridLayout3D.UpdateLayout();
    }

    #region

    public FarmTile BuyFarmTile()
    {
        var coinData = UserGameCoinData.Instance;
        if (coinData == null)
        {
            return null;
        }

        if (!coinData.IsCanUse(costBuyFarmTile))
        {
            return null;
        }

        var newFarmTileData = FarmTileDatas.AddFarmTileData();

        if (newFarmTileData == null)
        {
            return null;
        }

        var neWorker = CreateFarmTile(newFarmTileData);
        // Update 3D layout after creating all blocks
        gridLayout3D.UpdateLayout();

        return neWorker;
    }

    private FarmTile CreateFarmTile(GameFarmTileData tileData)
    {
        if (tileData == null)
        {
            Debug.LogError("Farm Tile Data null, can not create");
            return null;
        }

        var neFarmTile = SpawnObjectManager.Instance.CreateFarmTile();
        neFarmTile.SetUpFarmTile(tileData.FarmTileID);
        neFarmTile.transform.SetParent(gridContainTf);
        neFarmTile.gameObject.name = $"Tile_{tileData.FarmTileID}";

        tiles.Add(neFarmTile);
        OnBuyAFarmTile?.Invoke(neFarmTile);

        return neFarmTile;
    }

    public FarmTile GetFreeFarmTile()
    {
        var freeWorkerData = FarmTileDatas.GetFreeFarmTileData();

        if (freeWorkerData == null)
        {

            return null;
        }

        return GetFarmTileById(freeWorkerData.FarmTileID);
    }

    public FarmTile GetFarmTileById(int farmTileID)
    {
        FarmTile targetFarmTile = null;
        if (!dicFindFarmTile.TryGetValue(farmTileID, out targetFarmTile))
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].FarmID == farmTileID)
                {
                    targetFarmTile = tiles[i];
                    dicFindFarmTile.TryAdd(farmTileID, tiles[i]);
                    break;
                }
            }
        }
        return targetFarmTile;
    }


    #endregion

    #region UI

    public void Button_BuyFarmTile()
    {
        BuyFarmTile();
    }

    #endregion

}
