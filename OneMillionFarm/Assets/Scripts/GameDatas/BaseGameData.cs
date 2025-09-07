using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseGameData 
{
    public virtual void Init()
    { }
    public virtual void OpenGame()
    { }


    protected GameDataManager mainDataInstance;
    protected virtual void SaveData()
    {
        if (mainDataInstance == null)
        {
            mainDataInstance = GameDataManager.Instance;
        }
        if (mainDataInstance == null)
        {
            Debug.LogError("Data Manager Null, can not save");
            return;
        }
        mainDataInstance.SaveData();
    }
}
