using System;
using ResourceSystem;
using TMPro.EditorUtilities;
using UnityEngine;

namespace BuildingManage
{
    [CreateAssetMenu(menuName = "SO/Building")]
    public class BuildingSO : ScriptableObject
    {
        [HideInInspector] public string buildingTypeStr;
        [HideInInspector] public BuildingEnum fabricType = BuildingEnum.None;

        public string buildingName;
        public string description;
        
        public Building building;
        public int tileSize;        //타일맵에서의 사이즈

        public int health;          //건물의 체력

        public Resource[] needResource;
        
        private void OnEnable()
        {
            if (fabricType == BuildingEnum.None)
            {
                if (!Enum.TryParse(buildingTypeStr, out BuildingEnum building))
                {
                    Debug.LogError($"Enum Named {buildingTypeStr} is Not Exsist");
                    return;
                }

                fabricType = building;
            }
        }
    }
}