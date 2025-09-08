using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    [SerializeField] protected int objectID;

    [SerializeField] protected ItemType objectType;

    [SerializeField] protected BaseObjectAnimation objectAnimation;

    public ItemType ObjectType => objectType;

    public int ObjectID => this.objectID;

    public static System.Action<BaseObject, bool> OnInteractAction;

    protected virtual void Awake()
    {
        if (objectAnimation != null)
        {
            objectAnimation.Init(this);
        }
    }

    public virtual void DoInteractAction()
    {
        
    }

    public virtual void SetObjectID(int _objectID)
    {
        this.objectID = _objectID;
    }
}
