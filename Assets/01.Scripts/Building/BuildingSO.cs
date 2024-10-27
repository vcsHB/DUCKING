using System;
using TMPro.EditorUtilities;
using UnityEngine;

namespace BuildingManage
{
    [CreateAssetMenu(menuName = "SO/Building")]
    public class BuildingSO : ScriptableObject
    {
        public string buildingTypeStr;
        public BuildingEnum buildingType = BuildingEnum.None;

        public GameObject prefab;
        public Vector2Int tileSize;         //타일맵에서의 사이즈

        private void OnEnable()
        {
            if (buildingType == BuildingEnum.None)
            {
                if (!Enum.TryParse(buildingTypeStr, out BuildingEnum building))
                {
                    Debug.LogError($"Enum Named {buildingTypeStr} is Not Exsist");
                    return;
                }

                buildingType = building;
            }
        }
    }
}