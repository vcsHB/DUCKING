using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "SO/EnCorrosiumSO")]
public class EncorrosiumSO : ScriptableObject
{
    public int minSize, maxSize;
    public List<Shape> shapes = new List<Shape>();

    public TileBase corrosiumTile;
}

[Serializable]
public struct Shape
{
    public int minCount, maxCount;
    public int minSize, maxSize;
}