using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataManager : MonoSingleton<GameDataManager>
{
    [SerializeField] private GameStatsConfigs statsConfigs;
    [SerializeField] private GameAssetsConfigs assetsConfigs;

    private string KeySaveGameDatas = "UserFarmGameDatas";

    public static System.Action OnLoadDataDone;

    public GameStatsConfigs StatsConfigs => statsConfigs;
    public GameAssetsConfigs AssetsConfigs => assetsConfigs;

    private UserGameDatas userGameDatas;

    public UserGameDatas UserGameDatas
    {
        get
        {
            if (userGameDatas == null)
            {
                userGameDatas = new UserGameDatas();
                userGameDatas.Init();
            }

            return userGameDatas;
        }
    }

    public GameWorkerDatas WorkerDatas
    {
        get 
        {
            return UserGameDatas.WorkerDatas; 
        }
    }

    public GameFarmTileDatas FarmTileDatas
    {
        get
        {
            return UserGameDatas.FarmTileDatas;
        }
    }
    public GameCreatureDatas CreatureDatas
    {
        get
        {
            return UserGameDatas.CreatureDatas;
        }
    }

    public UserGameStatsData GameStatsData
    {
        get
        {
            return UserGameDatas.GameStatsData;
        }
    }

    public GameStorageItemDatas StorageItemDatas
    {
        get
        {
            return UserGameDatas.StorageItemDatas;
        }
    }

    public override void Init()
    {
        StartCoroutine(IE_LoadData());
    }

    private IEnumerator IE_LoadData()
    {
        InitReference();
        //Load Data
        LoadUserData();

        yield return new WaitForSeconds(0.2f);
        OnOpenGame();
        yield return new WaitForEndOfFrame();
        SaveData();
        yield return new WaitForSeconds(0.3f);
        OnLoadDataDone?.Invoke();
    }

    private void InitReference()
    {

    }

    private void CreateNewData()
    {
        userGameDatas = new UserGameDatas();
        userGameDatas.Init();
    }

    private void LoadUserData()
    {
        if (PlayerPrefs.HasKey(KeySaveGameDatas))
        {
            try
            {
                string saveStr = PlayerPrefs.GetString(this.KeySaveGameDatas);
                userGameDatas = JsonUtility.FromJson<UserGameDatas>(saveStr);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Get data error, create new data");
                Debug.LogException(e);
                CreateNewData();
            }
        }
        else
        {
            Debug.LogError("Dont have data, new user, create new data");
            CreateNewData();
        }
       
    }

    private void OnOpenGame()
    {
        UserGameDatas.OpenGame();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SaveData();
    }

    public void SaveData()
    {
        string saveStr = JsonUtility.ToJson(UserGameDatas, true);
        PlayerPrefs.SetString(KeySaveGameDatas, saveStr);
    }
}
