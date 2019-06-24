using System;
using UnityEngine;
using UnityEngine.Events;

public class FurnitureSelectionPanel : MonoBehaviour
{
    [SerializeField] private GameObject furniturePreviewPrefab;
    [SerializeField] private Transform furniturePreviewParent;

    [Tooltip("List of available furniture.")]
    [SerializeField] private FurnitureThumbnailPairList list;
    private FurniturePreview[] previews;

    ///<summary>Selected furniture in UI.</summary>
    private int selectionIndex = 0;

    public SelectionChangedEvent OnSelectionChangedEvent;

    private void Awake()
    {
        PopulateList();
    }

    /// <summary>
    /// Fills the scrollview with the available choices.
    /// </summary>
    private void PopulateList()
    {
        previews = new FurniturePreview[list.Pairs.Length];
        for (int i = 0; i < list.Pairs.Length; i++)
        {
            FurniturePreview furniturePreview = Instantiate(furniturePreviewPrefab, furniturePreviewParent).GetComponent<FurniturePreview>();
            previews[i] = furniturePreview;

            furniturePreview.SetData(list.Pairs[i].Thumbnail, i);
            furniturePreview.onSelectedEvent += OnFurnitureSelected;
        }

        previews[0].Select();
    }

    /// <summary>
    /// Update selection in UI and dispatch event of change.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="newIndex"></param>
    private void OnFurnitureSelected(object sender, int newIndex)
    {
        if (selectionIndex != newIndex)
        {
            previews[selectionIndex].Deselect();
            selectionIndex = newIndex;
            previews[selectionIndex].Select();

            OnSelectionChangedEvent?.Invoke(selectionIndex);
        }
    }

    [Serializable]
    public class SelectionChangedEvent : UnityEvent<int>
    {

    }
}