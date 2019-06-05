using UnityEngine;

/// <summary>
/// Rotates an object.
/// </summary>
public class TouchToRotate : MonoBehaviour
{
    private const string rotatingObjectTag = "Rotating Object";

    GameObject rotatingObject;
    Vector3 initialRotation;
    Vector2 initialTouchLocation;

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    FindObjectTouched(touch);
                    break;
                case TouchPhase.Moved:
                    RotateObject(touch);
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    ResetSelection();
                    break;
                case TouchPhase.Canceled:
                    ResetSelection();
                    break;
                default:
                    break;
            }
        }
    }

    private void ResetSelection()
    {
        rotatingObject = null;
    }

    private void FindObjectTouched(Touch touch)
    {
        ResetSelection();

        Ray ray = Camera.main.ScreenPointToRay(touch.position);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow, 2f);

        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider != null && raycastHit.transform.tag == rotatingObjectTag)
            {
                // save starting values to calculate rotation
                rotatingObject = raycastHit.transform.gameObject;
                initialRotation = rotatingObject.transform.localRotation.eulerAngles;
                initialTouchLocation = touch.position;
            }
            else
            {
                Debug.Log("Object isn't rotatable");
            }
        }
        else
        {
            Debug.Log("No hit");
        }
    }

    private void RotateObject(Touch touch)
    {
        if(rotatingObject != null)
        {
            float differenceX = initialTouchLocation.x - touch.position.x;
            rotatingObject.transform.rotation = Quaternion.Euler(initialRotation + (Vector3.up * differenceX));
        }
    }
}
