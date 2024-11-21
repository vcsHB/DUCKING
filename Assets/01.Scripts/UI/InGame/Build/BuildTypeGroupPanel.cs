using System.Collections.Generic;
using UnityEngine;

namespace UI.InGame.Build
{
    
    public class BuildTypeGroupPanel : MonoBehaviour
    {
        [SerializeField] private BuildingCategoryType _currentCategory;
        [SerializeField] private SerializeDictionary<BuildingCategoryType, BuildCategory> _buildingTypeDictionary;
        // 나중에 뭔가 기능확장을 시켜주면서 Dictionary를 확장 할 수도있음.
        [Header("essential Setting")] 
        [SerializeField] private BuildSelectPanel _buildSelectPanel;

        [SerializeField] private BuildTypeDescriptionPanel _descriptionPanel;
        [SerializeField] private Transform _contentTrm;
        [SerializeField] private BuildTypeSlot _slotPrefab;
        [SerializeField] private List<BuildTypeSlot> _slotList = new();
        
        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            foreach (var pair in _buildingTypeDictionary)
            {
                BuildTypeSlot slot = Instantiate(_slotPrefab, _contentTrm);
                slot.Initialize(this, pair.Key, pair.Value);
                _slotList.Add(slot);
            }
        }

        public void HandleSelectCategory(BuildCategory category)
        {
            _buildSelectPanel.SetDisplayBuildingSlots(category.buildingList);
        }

        public void HandleOnPointerDescription(BuildCategory category)
        {
            string description = $"{category.categoryName}\n{category.categoryDescription}";
            _descriptionPanel.ShowDescription(description);
        }

        public void HandleSetDescriptionActive(bool value)
        {
            _descriptionPanel.SetPanelActive(value);
        }

        public void SetDescPosition(float y)
        {
            _descriptionPanel.SetPositionY(y);
        }
    }
}