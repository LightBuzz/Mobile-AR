using System;
using UnityEngine;
using UnityEngine.UI;

public class FurniturePreview : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject selectedGO;

    [HideInInspector]
    public int furnitureIndex;

    public EventHandler<int> onSelectedEvent;

    public void SetData(Sprite newSprite, int newIndex)
    {
        image.sprite = newSprite;
        furnitureIndex = newIndex;
    }

    public void Deselect()
    {
        selectedGO.SetActive(false);
    }

    public void Select()
    {
        selectedGO.SetActive(true);
    }

    public void OnClick()
    {
        Select();

        onSelectedEvent?.Invoke(this, furnitureIndex);
    }
}
