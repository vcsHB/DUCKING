using System;
using ResourceSystem;
using TMPro.EditorUtilities;
using UnityEngine;

namespace BuildingManage
{
    [CreateAssetMenu(menuName = "SO/Building")]
    public class FabricSO : ScriptableObject
    {
        [HideInInspector] public string fabricTypeStr;
        [HideInInspector] public FabricEnum fabricType = FabricEnum.None;

        public string buildingName;
        public string description;
        
        public Fabric building;
        public int tileSize;        //타일맵에서의 사이즈

        public int health;          //건물의 체력

        public Resource[] needResource;
        
        private void OnEnable()
        {
            if (fabricType == FabricEnum.None)
            {
                if (!Enum.TryParse(fabricTypeStr, out FabricEnum building))
                {
                    Debug.LogError($"Enum Named {fabricTypeStr} is Not Exsist");
                    return;
                }

                fabricType = building;
            }
        }
    }
}