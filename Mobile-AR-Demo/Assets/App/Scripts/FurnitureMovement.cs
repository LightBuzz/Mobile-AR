using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FurnitureMovement : MonoBehaviour
{
    private ARRaycastManager raycastManager;

    private Furniture furnitureSelected;

    private Vector3 moveOffset;
    private Vector3 rotationInitialPosition;
    private Vector3 initialRotation;
    private bool isRotating = false;

    private float rotationModifier = 0.2f;

    public MovementActiveEvent onMovementActiveEvent;

    private void Awake()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    private void Update()
    {
        if(furnitureSelected != null)
            CheckInput();
    }

    private void CheckInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touchMove = Input.GetTouch(0);

            if (Input.touchCount == 1)
            {
                // Move
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                raycastManager.Raycast(touchMove.position, hits, TrackableType.Planes);

                if(hits.Count > 0)
                {
                    Pose pose = hits[0].pose;
                    
                    // Reset movement if the touch is new or if previously was rotating.
                    if (touchMove.phase == TouchPhase.Began || isRotating)
                    {
                        isRotating = false;
                        moveOffset = furnitureSelected.transform.position - pose.position;
                    }

                    furnitureSelected.transform.position = pose.position + moveOffset;
                }
            }
            else
            {
                // Rotate
                isRotating = true;
                Touch touchRotate = Input.GetTouch(1);
                
                if (touchRotate.phase == TouchPhase.Began)
                {
                    rotationInitialPosition = touchRotate.position;
                    initialRotation = furnitureSelected.transform.localRotation.eulerAngles;
                }

                furnitureSelected.transform.localRotation = Quaternion.Euler(0f, initialRotation.y + (rotationInitialPosition.y - touchRotate.position.y) * rotationModifier, 0f);
            }
        }
        else
        {
            furnitureSelected = null;

            onMovementActiveEvent?.Invoke(furnitureSelected == null);
        }
    }

    public void ObjectSelected(Transform objectTransform)
    {
        furnitureSelected = objectTransform.GetComponent<Furniture>();

        onMovementActiveEvent?.Invoke(furnitureSelected == null);

        CheckInput();
    }

    [Serializable]
    public class MovementActiveEvent : UnityEvent<bool>
    {

    }
}
