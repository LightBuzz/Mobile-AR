using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Places an object on a tracked surface.
/// </summary>
public class TapToPlace : MonoBehaviour
{
    private ARRaycastManager raycastManager;

    [SerializeField] private GameObject prefabToPlace;

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
                {
                    Pose pose = hits[0].pose;
                    Quaternion rotation = pose.rotation;
                    rotation *= Quaternion.Euler(Vector3.up * 180f); // look back.
                    Instantiate(prefabToPlace, pose.position, rotation);

                    Destroy(gameObject); // Don't spawn anything else
                }
                else
                {
                    Debug.Log("No surface found");
                }
            }
        }
    }
}
