using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatsPanel : MonoBehaviour
{
    protected virtual void Awake()
    {
        UnassignCallback();
        AssignCallback();
    }

    protected virtual void OnDestroy()
    {
        UnassignCallback();
    }

    protected virtual void AssignCallback()
    {

    }

    protected virtual void UnassignCallback()
    {

    }
}
