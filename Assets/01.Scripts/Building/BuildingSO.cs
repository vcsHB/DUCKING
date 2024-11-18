using System;
using ResourceSystem;
using UnityEngine;

namespace BuildingManage
{
    [CreateAssetMenu(menuName = "SO/Building")]
    public class BuildingSO : ScriptableObject
    {
        public string buildingTypeStr;
        public BuildingEnum buildingType = BuildingEnum.None;

        public string buildingName;
        public string description;
        public Sprite buildingIconSprite;   
        public Building building;
        public int tileSize;        //타일맵에서의 사이즈
        public bool canRotate;

        public int health;          //건물의 체력

        public Resource[] needResource;
        
        private void OnEnable()
        {
            if (buildingType == BuildingEnum.None)
            {
                Enum.TryParse(buildingTypeStr, out buildingType);
            }
        }
    }
}