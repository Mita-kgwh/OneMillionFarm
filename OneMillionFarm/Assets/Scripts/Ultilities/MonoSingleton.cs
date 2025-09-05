using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance = null;
    private static bool shuttingDown = false;
    public static T Instance
    {
        get
        {
            if (_instance == null && !shuttingDown && Application.isPlaying)
            {
                _instance = FindObjectOfType(typeof(T)) as T;

                if (_instance == null)
                {
                    Debug.LogWarning("No instance of " + typeof(T).ToString() + ", a temporary one is created.");

                    _instance = new GameObject("Temp Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                }
            }    
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
        }
        else if (_instance != this)
        {
            DestroyImmediate(gameObject);
        }

        Init();
    }

    public virtual void Init()
    {

    }

    protected virtual void OnDestroy()
    {
        if (this == _instance)
            _instance = null;
    }

    private void OnApplicationQuit()
    {
        _instance = null;
        shuttingDown = true;
    }
}
