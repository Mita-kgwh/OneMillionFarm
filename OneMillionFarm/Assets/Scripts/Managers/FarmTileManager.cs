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

    public override void Init()
    {
        base.Init();
        UnassignCallback();
        AssignCallback();
        var storeCf = StoreItemConfigs.Instance.GetStoreItemConfigByType(ItemType.FARMTILE);
        if (storeCf != null)
        {
            this.costBuyFarmTile = storeCf.TradingValue;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UnassignCallback();
    }

    private void AssignCallback()
    {
        CreaturesManager.OnCreateACreature += OnCreateACreatureCallback;
    }

    private void UnassignCallback()
    {
        CreaturesManager.OnCreateACreature -= OnCreateACreatureCallback;
    }

    #region Callback

    private void OnCreateACreatureCallback(BaseCreatureItem neCreature)
    {
        if (neCreature == null)
        {
            return;
        }

        var tile = GetFarmTileById(neCreature.FarmID);
        if (tile == null)
        {
            //Debug.LogError($"Farm tile null {neCreature.FarmID} when assign creature");
            return;
        }

        neCreature.transform.position = tile.transform.position;
        tile.AssignCreatureItem(neCreature);
    }

    #endregion

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

        DelayUpdateLayout();
    }

    private void DelayUpdateLayout()
    {
        StartCoroutine(IE_UpdateLayout());
    } 

    private IEnumerator IE_UpdateLayout()
    {
        yield return new WaitForSeconds(0.3f);
        // Update 3D layout after creating all blocks
        gridLayout3D.UpdateLayout();
    }

    public void RestartGame()
    {
        var spawnManager = SpawnObjectManager.Instance;
        for (int i = 0; i < tiles.Count; i++)
        {
            spawnManager.Return2Pool(tiles[i]);
        }

        tiles.Clear();
        this.farmTileDatas = null;
        this.dicFindFarmTile.Clear();

        DelayUpdateLayout();
    }

    #region

    public FarmTile BuyFarmTile()
    {
        var coinData = UserGameStatsData.Instance;
        if (coinData == null)
        {
            return null;
        }

        if (!coinData.IsCanUse(costBuyFarmTile))
        {
            return null;
        }

        coinData.UseCoin(costBuyFarmTile);

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
        if (neFarmTile == null)
        {
            Debug.LogError("SpawnObjectManager spawn null FarmTile");
            return null;
        }
        neFarmTile.SetUpFarmTile(tileData);
        neFarmTile.transform.SetParent(gridContainTf);
        neFarmTile.gameObject.name = $"Tile_{tileData.FarmTileID}";

        tiles.Add(neFarmTile);
        OnBuyAFarmTile?.Invoke(neFarmTile);

        return neFarmTile;
    }

    public FarmTile GetFreeFarmTile()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].IsFree)
            {
                return tiles[i];
            }
        }

        return null;
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
