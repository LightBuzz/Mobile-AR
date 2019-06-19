using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Listeners to any boundary change of the plane and generates a wall around it.
/// </summary>
[RequireComponent(typeof(ARPlane))]
public class PlaneWallGenerator : MonoBehaviour
{
    ARPlane plane;

    private Wall wall;

    private const float wallHeight = 1f;

    private void Awake()
    {
        wall = GetComponentInChildren<Wall>();

        plane = GetComponent<ARPlane>();
        plane.boundaryChanged += OnBoundaryChanged;
    }

    private void OnDestroy()
    {
        plane.boundaryChanged -= OnBoundaryChanged;

        Furniture[] furnitures = FindObjectsOfType<Furniture>();

        foreach (Furniture item in furnitures)
            item.RemoveCollision(gameObject.name);
    }

    private void OnBoundaryChanged(ARPlaneBoundaryChangedEventArgs eventArgs)
    {
        // Unsuported plane alignment
        if (plane.alignment != UnityEngine.XR.ARSubsystems.PlaneAlignment.HorizontalUp)
            return;

        GenerateWallMesh(plane.boundary);
    }

    /// <summary>
    /// Create a new wall mesh for the boundary.
    /// </summary>
    /// <param name="boundary"></param>
    public void GenerateWallMesh(NativeArray<Vector2> boundary)
    {
        CombineInstance[] meshes = new CombineInstance[boundary.Length - 1];

        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i] = GenerateQuad(boundary, i);
        }
        
        wall.ChangeMesh(meshes);
    }

#if UNITY_EDITOR
    /// <summary>
    /// Deletes mesh colliders in the wall game object.
    /// </summary>
    public void DestroyWallMeshColliders()
    {
        wall = GetComponentInChildren<Wall>();
        wall.DestroyWallMeshColliders();
    }
#endif

    /// <summary>
    /// Generates a quad for a straight line in the boundary referenced by the meshIndex.
    /// </summary>
    /// <param name="boundary"></param>
    /// <param name="meshIndex"></param>
    /// <returns></returns>
    private CombineInstance GenerateQuad(NativeArray<Vector2> boundary, int meshIndex)
    {
        Vector3[] vertices = new Vector3[4];
        for (int i = meshIndex; i < meshIndex + 2; i++)
        {
            vertices[(i - meshIndex) * 2] = new Vector3(boundary[i].x, 0f, boundary[i].y);
            vertices[((i - meshIndex) * 2) + 1] = new Vector3(boundary[i].x, 0f + wallHeight, boundary[i].y);
        }

        int[] triangles = new int[6];

        triangles[0] = 0;
        triangles[1] = 3;
        triangles[2] = 1;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.name = "Mesh_" + meshIndex;

        CombineInstance ci = new CombineInstance();
        ci.mesh = mesh;

        return ci;
    }
}
