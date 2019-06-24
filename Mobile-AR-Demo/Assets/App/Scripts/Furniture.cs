using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    private FurnitureIndicator indicator;

    private HashSet<string> collisionWalls;
    
    public MeshFilter FurnitureMeshFilter { private set; get; }

    private bool showIndicator = true;

    private void Awake()
    {
        FurnitureMeshFilter = GetComponent<MeshFilter>();
        indicator = GetComponentInChildren<FurnitureIndicator>();

        collisionWalls = new HashSet<string>();

        // Hide the mesh before it's placed
        FurnitureMeshFilter.GetComponent<MeshRenderer>().enabled = false;
        ShowIndicator(true);
    }

    /// <summary>
    /// Adds collision from a colliding object.
    /// </summary>
    /// <param name="wallName"></param>
    public void AddCollision(string wallName)
    {
        collisionWalls.Add(wallName);

        UpdateIndicator();
    }

    /// <summary>
    /// Removes collision from a colliding object.
    /// </summary>
    /// <param name="wallName"></param>
    public void RemoveCollision(string wallName)
    {
        collisionWalls.Remove(wallName);

        UpdateIndicator();
    }

    private void OnTriggerEnter(Collider other)
    {
        Furniture furniture = other.GetComponent<Furniture>();

        if (furniture != null)
            furniture.AddCollision(gameObject.name);
        // the other object will update this
    }

    private void OnTriggerExit(Collider other)
    {
        Furniture furniture = other.GetComponent<Furniture>();
        if (furniture != null)
            furniture.RemoveCollision(gameObject.name);
        // the other object will update this.
    }

    public void Place()
    {
        FurnitureMeshFilter.GetComponent<MeshRenderer>().enabled = true;
        SetLayerRecursively(LayerMask.NameToLayer("Furniture"));

        Debug.Log("Placed " + gameObject.name);
    }

    private void SetLayerRecursively(int layerIndex)
    {
        gameObject.layer = layerIndex;

        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < transforms.Length; i++)
            transforms[i].gameObject.layer = layerIndex;
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
            if (collisionWalls.Count > 0)
                indicator.ShowCollision();
            else
                indicator.ShowPlacement();
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
