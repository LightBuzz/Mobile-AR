using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    private FurnitureIndicator indicator;

    private bool placed = false;

    private HashSet<string> collisionWalls;
    
    public MeshFilter FurnitureMeshFilter { private set; get; }

    public bool showIndicator = true;

    private void Awake()
    {
        FurnitureMeshFilter = GetComponent<MeshFilter>();
        indicator = GetComponentInChildren<FurnitureIndicator>();

        collisionWalls = new HashSet<string>();
    }

    public void AddCollision(string wallName)
    {
        collisionWalls.Add(wallName);

        UpdateIndicator();
    }

    public void RemoveCollision(string wallName)
    {
        collisionWalls.Remove(wallName);

        UpdateIndicator();
    }

    public void Place()
    {
        FurnitureMeshFilter.GetComponent<MeshRenderer>().enabled = true;
        SetLayerRecursively(LayerMask.NameToLayer("Furniture"));
    }

    private void SetLayerRecursively(int layerIndex)
    {
        gameObject.layer = layerIndex;

        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].gameObject.layer = layerIndex;
        }
    }

    public void ShowIndicator(bool show)
    {
        showIndicator = show;

        UpdateIndicator();
    }

    private void UpdateIndicator()
    {
        if (showIndicator)
        {
            if (placed)
            {
                if (collisionWalls.Count > 0)
                    indicator.ShowCollision();
                else
                    indicator.Hide();
            }
            else
            {
                if (collisionWalls.Count > 0)
                    indicator.ShowCollision();
                else
                    indicator.ShowPlacement();
            }
        }
        else
        {
            indicator.Hide();
        }
    }

    public bool IsColliding()
    {
        return collisionWalls.Count > 0;
    }
}
