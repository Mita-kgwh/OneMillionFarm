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

    protected virtual void SaveData()
    {

    }
}
