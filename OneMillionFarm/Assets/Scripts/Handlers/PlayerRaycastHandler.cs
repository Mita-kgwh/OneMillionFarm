using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class ObjectLayerRaycast
{
    public static string RAYCASTABLE = "Raycastable";
    public static string UNRAYCASTABLE = "Unraycastable";
    public static string BLOCKER = "Blocker";
}

public class PlayerRaycastHandler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private bool blockRaycast;
    private LayerMask targetlayerMask;

    public static System.Action<BaseObject> OnRaycastHitObject;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        targetlayerMask = LayerMask.GetMask(ObjectLayerRaycast.RAYCASTABLE);
        UnassignCallback();
        AssignCallback();
    }

    private void OnDestroy()
    {
        UnassignCallback();
    }

    private void AssignCallback()
    {

    }

    private void UnassignCallback()
    {

    }

    public void SetTargetLayerMask()
    {

    }


    private void Update()
    {
        if (blockRaycast)
            return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            RaycastObject();
        }
#else
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;
            if (touch.phase == TouchPhase.Began)
            {
                RaycastObject();
            }
        }
#endif
    }

    public void RaycastObject()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 500f, LayerMask.GetMask(ObjectLayerRaycast.BLOCKER)))
        {
            return;
        }

        Physics.SyncTransforms();

        if (Physics.Raycast(ray, out hit, 500f, targetlayerMask))
        {
            if (hit.transform.gameObject.TryGetComponent(out BaseObject hitObject))
            {
                //OnRaycastHitObject?.Invoke(hitObject);
                hitObject.DoInteractAction();
            }
        }
    }
}
