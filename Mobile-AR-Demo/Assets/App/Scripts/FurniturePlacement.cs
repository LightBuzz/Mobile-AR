using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FurniturePlacement : MonoBehaviour
{
    private GameObject prefabToPlace;

    private ARPlaneManager planeManager;

    private Furniture activeFurniture;

    [SerializeField] private FurnitureThumbnailPairList list;

    private void Start()
    {
        planeManager = FindObjectOfType<ARPlaneManager>();

        UpdateActiveFurniture(0);
    }

    public void OnSelectionChanged(int newIndex)
    {
        UpdateActiveFurniture(newIndex);
    }

    private void UpdateActiveFurniture(int index)
    {
        if(activeFurniture != null)
        {
            Destroy(activeFurniture.gameObject);

            activeFurniture = null;
        }

        prefabToPlace = list.Pairs[index].Model;

        CreateNewActiveFurniture();
    }

    private void CreateNewActiveFurniture()
    {
        activeFurniture = Instantiate(prefabToPlace).GetComponent<Furniture>();
        activeFurniture.name += Guid.NewGuid(); // unique name
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
            }
        }
        else
        {
            Debug.Log("Prefab doesn't fit to surface");
        }
    }

    private bool CheckIfSurfaceIsBigEnough(Mesh mesh, ARPlane plane)
    {
        float meshSize = mesh.bounds.size.x * mesh.bounds.size.z;
        float planeSize = plane.size.x * plane.size.y;
        return meshSize < planeSize;
    }
}