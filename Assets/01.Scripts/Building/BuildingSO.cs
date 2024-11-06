using System;
using ResourceSystem;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace BuildingManage
{
    [CreateAssetMenu(menuName = "SO/Building")]
    public class BuildingSO : ScriptableObject
    {
        public string buildingTypeStr;
        public BuildingEnum buildingType = BuildingEnum.None;

        public string buildingName;
        public string description;
        
        public Building building;
        public int tileSize;        //타일맵에서의 사이즈

        public int health;          //건물의 체력

        public Resource[] needResource;
        
        private void OnEnable()
        {
            if (buildingType == BuildingEnum.None)
            {
                Debug.Log((name));
                if (!Enum.TryParse(buildingTypeStr, out BuildingEnum building))
                {
                    Debug.LogError($"Enum Named {buildingTypeStr} is Not Exist");
                    return;
                }
                Debug.Log("파싱 성공");
                buildingType = building;
            }
        }
    }
}