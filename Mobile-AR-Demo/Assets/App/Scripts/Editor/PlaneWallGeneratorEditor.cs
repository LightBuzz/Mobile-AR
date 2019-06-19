using Unity.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlaneWallGenerator))]
public class PlaneWallGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlaneWallGenerator script = (PlaneWallGenerator)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Test random"))
        {
            NativeArray<Vector2> boundary = new NativeArray<Vector2>(new Vector2[] {
                new Vector2(0f, 0f),
                new Vector2(0f, 10f),
                new Vector2(10f, 10f),
                new Vector2(10f, 0f),
                new Vector2(0f, 0f)
            }, Allocator.Temp );

            // Clear existing mesh colliders
            script.DestroyWallMeshColliders();

            script.GenerateWallMesh(boundary);
        }
    }
}