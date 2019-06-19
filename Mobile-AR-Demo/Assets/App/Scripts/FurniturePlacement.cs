using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FurniturePlacement : MonoBehaviour
{
    [SerializeField] private GameObject prefabToPlace;

    private ARPlaneManager planeManager;

    private Furniture activeFurniture;

    private void Start()
    {
        planeManager = FindObjectOfType<ARPlaneManager>();

        CreateNewActiveFurniture();
    }

    private void CreateNewActiveFurniture()
    {
        activeFurniture = Instantiate(prefabToPlace).GetComponent<Furniture>();
    }

    public void EnableIndicator(bool show)
    {
        activeFurniture.ShowIndicator(show);
    }

    public void UpdateActiveObject(bool isValid, Pose pose)
    {
        activeFurniture.gameObject.SetActive(isValid);

        activeFurniture.transform.SetPositionAndRotation(pose.position, pose.rotation);
    }

    public void PlaceFurniture(ARRaycastHit raycastHit)
    {
        ARPlane plane = planeManager.GetPlane(raycastHit.trackableId);
        if(plane.alignment != UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
        {
            Debug.Log("Incompatible plane allignment " + plane.alignment);
            return;
        }

        // does the selected prefab fit?
        bool fits = CheckIfSurfaceIsBigEnough(activeFurniture.FurnitureMeshFilter.mesh, plane);

        if(fits)
        {
            if(activeFurniture.IsColliding())
            {
                Debug.Log("Can't place due to collision");
            }
            else
            {
                activeFurniture.Place();
                CreateNewActiveFurniture();
                Debug.Log("Placed prefab");
            }
        }
        else
        {
            Debug.Log("prefab doesn't fit to surface");
        }
    }

    private bool CheckIfSurfaceIsBigEnough(Mesh mesh, ARPlane plane)
    {
        float meshSize = mesh.bounds.size.x * mesh.bounds.size.z;
        float planeSize = plane.size.x * plane.size.y;
        return meshSize < planeSize;
    }
}