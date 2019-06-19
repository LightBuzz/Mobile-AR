using UnityEngine;

public class FurnitureIndicator : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private static readonly Color placementColor = Color.blue;
    private static readonly Color collisionColor = Color.red;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ShowPlacement()
    {
        gameObject.SetActive(true);
        meshRenderer.material.color = placementColor;
    }

    public void ShowCollision()
    {
        gameObject.SetActive(true);
        meshRenderer.material.color = collisionColor;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}