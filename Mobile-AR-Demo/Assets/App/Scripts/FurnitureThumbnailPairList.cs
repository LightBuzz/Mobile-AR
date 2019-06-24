using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureThumbnailPairList", menuName ="Furniture Thumbnails Pair List")]
public class FurnitureThumbnailPairList : ScriptableObject
{
    [SerializeField] private FurnitureThumbnailPair[] pairs;
    public FurnitureThumbnailPair[] Pairs { get { return pairs; } }
}

[Serializable]
public class FurnitureThumbnailPair
{
    [SerializeField] private GameObject model;
    public GameObject Model { get { return model; } }

    [SerializeField] private Sprite thumbnail;
    public Sprite Thumbnail { get { return thumbnail; } }
}