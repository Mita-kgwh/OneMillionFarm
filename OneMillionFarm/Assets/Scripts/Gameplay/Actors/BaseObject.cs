using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    [SerializeField] protected int objectID;

    [SerializeField] protected ItemType objectType;
    public ItemType ObjectType => objectType;

    public static System.Action<ItemType, bool> OnInteractAction;

    public virtual void DoInteractAction()
    {
        
    }

    public virtual void SetObjectID(int _objectID)
    {
        this.objectID = _objectID;
    }
}
