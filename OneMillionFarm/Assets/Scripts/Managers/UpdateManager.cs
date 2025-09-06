using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoSingleton<UpdateManager>
{
    private List<IUpdateable> updateableObjs = new List<IUpdateable>();

    public void RegisterUpdateableObject(IUpdateable obj)
    {
        if (updateableObjs.Contains(obj))
        {
            return;
        }
        updateableObjs.Add(obj);
    }

    public void UnregisterUpdateableObject(IUpdateable obj)
    {
        if (!updateableObjs.Contains(obj))
        {
            return;
        }
        updateableObjs.Remove(obj);
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        for (int i = 0; i < updateableObjs.Count; i++)
        {
            updateableObjs[i].OnUpdate(dt);
        }
    }

}
