using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates the wall mesh and listens to collisions with Furniture.
/// </summary>
public class Wall : MonoBehaviour
{
    /// <summary>
    /// Boundary colliders.
    /// </summary>
    private List<MeshCollider> meshColliders;
    ///<summary>Mesh filter to visualise wall.</summary>
    private MeshFilter meshFilter;
    ///<summary>Generated mesh cache.</summary>
    Mesh mesh;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Furniture furniture = other.GetComponent<Furniture>();
        if (furniture != null)
            furniture.AddCollision(gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Furniture furniture = other.GetComponent<Furniture>();
        if (furniture != null)
            furniture.RemoveCollision(gameObject.name);
    }

    public void ChangeMesh(CombineInstance[] meshes)
    {
        if (mesh == null)
            mesh = new Mesh();
        mesh.Clear();
        mesh.CombineMeshes(meshes, true, false);

        meshFilter.sharedMesh = mesh;

        // Generate colliders
        if (meshColliders == null)
            meshColliders = new List<MeshCollider>();

        // Generate as many colliders as needed.
        while (meshColliders.Count < meshes.Length)
        {
            MeshCollider newCollider = meshFilter.gameObject.AddComponent<MeshCollider>();
            newCollider.convex = true;
            newCollider.isTrigger = true;
            meshColliders.Add(newCollider);
        }

        // Enable or disable only used colliders
        for (int i = 0; i < meshColliders.Count; i++)
        {
            if (i < meshes.Length)
            {
                meshColliders[i].enabled = true;
                meshColliders[i].sharedMesh = meshes[i].mesh;

                //ERROR: In some cases Simplex input points appers to be coplanar.
                //Debug.Log("vertices with scale " + transform.lossyScale);
                //for (int v = 0; v < meshes[i].mesh.vertices.Length; v++)
                //    Debug.Log(meshes[i].mesh.vertices[v]);
            }
            else
            {
                meshColliders[i].enabled = false;
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Deletes mesh colliders in the wall game object.
    /// </summary>
    public void DestroyWallMeshColliders()
    {
        meshFilter = GetComponent<MeshFilter>();
        MeshCollider[] colliders = meshFilter.GetComponents<MeshCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            DestroyImmediate(colliders[i]);
        }
        meshColliders = null;
        Debug.Log("Destroyed " + colliders.Length + " colliders");
    }
#endif
}
