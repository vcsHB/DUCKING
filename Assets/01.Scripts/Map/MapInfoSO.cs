using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "SO/MapInfo")]
public class MapInfoSO : ScriptableObject
{
    public Vector2Int mapSize;
    public FloorSO floorInfo;
    public List<FabricInfo> fabrics;
}

[Serializable]
public struct FabricInfo
{
    public GameObject fabricPf;
    public float fabricRadius, fabricOffset;
    public int minSpawnCnt, maxSpawnCnt;
}
