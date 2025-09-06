using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plant or Animal on Plant Tile
/// </summary>
public class BaseCreatureItem : BaseObject, IUpdateable
{
    protected BaseCreatureBehaviour creatureBehaviour;
    protected float timer;
    protected int currentAmountProduct;

    public int CreatureID => this.objectID;

    public override void DoInteractAction()
    {
        base.DoInteractAction();
        Debug.Log($"This is Creature {this.objectID}");
    }

    private void Start()
    {
        UpdateManager.Instance.RegisterUpdateableObject(this);
    }

    private void OnDestroy()
    {
        if(UpdateManager.Instance != null )
        {
            UpdateManager.Instance.UnregisterUpdateableObject(this);
        }
    }

    public virtual void OnUpdate(float dt)
    {
        timer -= dt;
        if (timer <= 0)
        {
            OnEndTimeDo();
        }
    }

    public virtual void StartTimer()
    {
        ///Get timer
    }

    protected virtual void OnEndTimeDo()
    {
        ///
    }
}
