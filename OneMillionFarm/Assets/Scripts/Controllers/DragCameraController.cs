using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UIElements;

public class DragCameraController : MonoBehaviour
{
    [Header("Swipe Settings")]
    [SerializeField] private float minSwipeDistance = 50f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform worldObjTf;
    // Events
    public static event Action<Vector3> OnSwipeStart;
    public static event Action<Vector3> OnSwipeUpdate; 
    public static event Action<Vector3, Vector3> OnSwipeEnd; // start pos, end pos, direction
    public static event Action OnSwipeCancel;

    // Input tracking
    private bool blockDrag = false;
    private bool isSwipeActive = false;
    private Vector3 swipeStartPosition;
    private Vector3 currentSwipePosition;
    private float swipeStartTime;

    // World position conversion
    private Vector3 rootWorldPos;
    private Vector3 swipeStartWorldPos;
    private Vector3 currentSwipeWorldPos;

    /// <summary>
    /// World Position of drag line will be {offsetY} distance from board plane
    /// </summary>
    public float offsetY = 6f;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        UnassignCallback();
        AssignCallback();
    }

    private void OnDestroy()
    {
        // Clean up events
        OnSwipeStart = null;
        OnSwipeUpdate = null;
        OnSwipeEnd = null;
        OnSwipeCancel = null;

        UnassignCallback();
    }

    private void AssignCallback()
    {
        BaseDialog.OnShowDialog += OnShowDialogCallback;
        BaseDialog.OnHideDialog += OnHideDialogCallback;
    }

    private void UnassignCallback()
    {

    }

    private void OnShowDialogCallback(BaseDialog baseDialog)
    {
        blockDrag = true;
    }

    private void OnHideDialogCallback(BaseDialog baseDialog)
    {
        blockDrag = false;
    }

    private void Update()
    {
        if (blockDrag)
        {
            return;
        }
        HandleInput();
    }

    private void HandleInput()
    {
        // Handle touch input for mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            HandleTouchInput(touch);
        }
        // Handle mouse input for desktop testing
        else
        {
            HandleMouseInput();
        }
    }

    private void HandleTouchInput(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                StartSwipe(touch.position);
                break;

            case TouchPhase.Moved:
                UpdateSwipe(touch.position);
                break;

            case TouchPhase.Ended:
                EndSwipe(touch.position);
                break;

            case TouchPhase.Canceled:
                CancelSwipe();
                break;
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSwipe(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && isSwipeActive)
        {
            UpdateSwipe(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && isSwipeActive)
        {
            EndSwipe(Input.mousePosition);
        }
    }

    private void StartSwipe(Vector3 screenPosition)
    {
        if (isSwipeActive) return;

        isSwipeActive = true;
        swipeStartPosition = screenPosition;
        currentSwipePosition = screenPosition;
        swipeStartTime = Time.time;

        // Convert to world position
        //swipeStartWorldPos = ScreenToWorldPosition(screenPosition);
        rootWorldPos = worldObjTf.transform.position;
        swipeStartWorldPos = ScreenToWorldPointWCamOrthographic(screenPosition);
        currentSwipeWorldPos = swipeStartWorldPos;

        //Debug.LogError($"Start Swipe {swipeStartWorldPos}");
        OnSwipeStart?.Invoke(swipeStartWorldPos);
    }

    private void UpdateSwipe(Vector3 screenPosition)
    {
        if (!isSwipeActive || blockDrag) return;

        currentSwipePosition = screenPosition;
        //currentSwipeWorldPos = ScreenToWorldPosition(screenPosition);
        currentSwipeWorldPos = ScreenToWorldPointWCamOrthographic(screenPosition);

        float swipeDistance = Vector3.Distance(currentSwipeWorldPos, swipeStartWorldPos);
        // Calculate final swipe direction
        Vector3 swipeVector = currentSwipeWorldPos - swipeStartWorldPos;
        //Debug.LogError($"{currentSwipeWorldPos} - {swipeStartWorldPos}");
        worldObjTf.transform.position = rootWorldPos + swipeVector;

        OnSwipeUpdate?.Invoke(currentSwipeWorldPos);
    }

    private void EndSwipe(Vector3 screenPosition)
    {
        if (!isSwipeActive) return;

        currentSwipePosition = screenPosition;
        currentSwipeWorldPos = ScreenToWorldPosition(screenPosition);

        float swipeTime = Time.time - swipeStartTime;
        float swipeDistance = Vector3.Distance(swipeStartPosition, currentSwipePosition);

        // Calculate final swipe direction
        Vector3 swipeVector = currentSwipePosition - swipeStartPosition;

        // Validate swipe
        if (!blockDrag && IsValidSwipe(swipeDistance, swipeTime))
        {
            //Debug.Log($"EndSwipe Valid - Direction: {direction}");
            OnSwipeEnd?.Invoke(swipeStartWorldPos, currentSwipeWorldPos);
        }
        else
        {
            //Debug.Log("EndSwipe Cancel");
            OnSwipeCancel?.Invoke();
        }

        ResetSwipe();
    }

    private Vector3 ScreenToWorldPointWCamOrthographic(Vector3 screenPos)
    {
        //If wrong drag(touch point far distance than drag object on screen)
        //Check cam, there a second cam turn on/off on screen make it wrong
        var camMain = Camera.main;
        if (camMain == null)
        {
            Debug.LogError("Null Main Cam");
            return Vector3.zero;
        }
        if (!camMain.name.Equals("Main Camera"))
        {
            Debug.LogError("Check cam main, seem like we get the wrong one!");
        }
        var worldPos = camMain.ScreenToWorldPoint(screenPos);
        //var orgPos = worldPos;
        var camForward = camMain.transform.forward;
        float scale = Mathf.Abs((worldPos.y - offsetY) / camForward.y);

        worldPos = worldPos + camForward * scale;
        //Debug.LogError($"ScreenPos: {screenPos}, OrgWorldPos: {orgPos}, Camfwd:{camForward}, NewWorldPos: {worldPos}");
        return worldPos;
    }


    private void CancelSwipe()
    {
        if (!isSwipeActive) return;

        OnSwipeCancel?.Invoke();
        ResetSwipe();
    }

    private bool IsValidSwipe(float distance, float time)
    {
        return distance >= minSwipeDistance; //&& time <= maxSwipeTime
    }

    private void ResetSwipe()
    {
        isSwipeActive = false;
        swipeStartPosition = Vector3.zero;
        currentSwipePosition = Vector3.zero;
        swipeStartTime = 0f;
    }

    private Vector3 ScreenToWorldPosition(Vector3 screenPosition)
    {
        if (mainCamera == null) return Vector3.zero;

        // Convert screen position to world position on the game plane (z = 0)
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, mainCamera.nearClipPlane));
        worldPosition.z = 0f; // Ensure we're on the game plane

        return worldPosition;
    }
}
