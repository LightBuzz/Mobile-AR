using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Handles generic touch of the app.
/// </summary>
public class TouchManager : MonoBehaviour
{
    private ARRaycastManager raycastManager;

    #region EVENTS
    public RaycastHitEvent onPlaneHitEvent;
    ///<summary>Event that is called every frame updating the placement preview.</summary>
    public PoseHitEvent onCameraPoseUpdateEvent;
    public ObjectSelectedEvent onObjectSelectedEvent;
    #endregion

    private void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    private void Update()
    {
        UpdateCameraPose();
        CheckInput();
    }

    /// <summary>
    /// Checks where the camera is looking at and dispatches an event with the target.
    /// </summary>
    private void UpdateCameraPose()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        bool isValid = hits.Count > 0;

        if (isValid)
        {
            Pose pose = hits[0].pose;

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraBearing = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;

            pose.rotation = Quaternion.LookRotation(cameraBearing);

            onCameraPoseUpdateEvent?.Invoke(isValid, pose);
        }
        else
        {
            onCameraPoseUpdateEvent?.Invoke(isValid, new Pose());
        }
    }

    /// <summary>
    /// Checks if there is input and if any interaction was done.
    /// </summary>
    private void CheckInput()
    {
        // Debug in editor
        if (Application.isEditor)
        {
            if (Input.anyKeyDown)
            {
                Transform hitObject = FindObjectTouched(Camera.main.ScreenPointToRay(Input.mousePosition));
                if (hitObject)
                {
                    // move object
                    Debug.Log("Object hit " + hitObject.name);
                }
                else
                {
                    Debug.Log("No hit");
                }
            }
        }

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase ==  TouchPhase.Began)
            {
                Transform hitObject = FindObjectTouched(Camera.main.ScreenPointToRay(touch.position));

                if(hitObject)
                    onObjectSelectedEvent?.Invoke(hitObject);
                else
                    CheckIfPlaneWasHit(touch);
            }
        }
    }

    private void CheckIfPlaneWasHit(Touch touch)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        // dispatch event to spawn object
        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinBounds))
            onPlaneHitEvent?.Invoke(hits[0]);
        else
            Debug.Log("No surface found");
    }

    /// <summary>
    /// Checks whether an object was touched and returns it.
    /// </summary>
    /// <param name="ray"></param>
    /// <returns>Transform of object touched. Null if none.</returns>
    private Transform FindObjectTouched(Ray ray)
    {
        LayerMask layerMask = LayerMask.GetMask("Furniture");
        
        RaycastHit raycastHit;
        Debug.DrawLine(ray.origin, ray.direction * 100f, Color.red, 2f);
        if(Physics.Raycast(ray, out raycastHit, 100f, layerMask))
        {
            if(raycastHit.collider != null)
                return raycastHit.transform;
        }

        return null;
    }
    
    [Serializable]
    public class RaycastHitEvent : UnityEvent<ARRaycastHit>
    {

    }

    [Serializable]
    public class PoseHitEvent : UnityEvent<bool, Pose>
    {

    }

    [Serializable]
    public class ObjectSelectedEvent : UnityEvent<Transform>
    {

    }
}
