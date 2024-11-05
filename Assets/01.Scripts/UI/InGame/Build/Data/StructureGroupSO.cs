using BuildingManage;
using UnityEngine;

namespace UI.InGame.Build
{
    public enum BuildingCategoryType
    {
        Transform,
        Mining,
        Tower,
        Factory,
        Wall,
        Utility
        
    }

    [System.Serializable]
    public class BuildCategory
    {
        public Sprite categoryIconSprite;
        public string categoryName;
        public string categoryDescription;
        public FabricSO[] buildingList;
    }
    
    public class StructureGroupSO : ScriptableObject
    { // 나중에 어딘가 폴더링을 옮겨야 될지도
        // UI에 있기 애매할 수 있음
        public SerializeDictionary<BuildingCategoryType, BuildCategory> buildingDictionary;

        public BuildCategory GetCategory(BuildingCategoryType buildingCategoryType)
        {
            if (buildingDictionary.TryGetValue(buildingCategoryType, out BuildCategory category))
            {
                return category;
            }
            Debug.LogError("빌딩 타입이 유효하지 않음.");
            return null;
        }
    }
}