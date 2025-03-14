using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingManage
{
    [CreateAssetMenu(menuName = "SO/MapInfo")]
    public class MapInfoSO : ScriptableObject
    {
        public Vector2Int mapSize;
        public BiomSO biomInfo;
        public List<BuildingInfo> fabrics;
    }

    [Serializable]
    public struct BuildingInfo
    {
        public BuildingEnum fabric;
        public float fabricOffset;
        public int minSpawnCnt, maxSpawnCnt;
    }
}



