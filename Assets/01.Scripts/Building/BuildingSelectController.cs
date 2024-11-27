using InputManage;
using UI.InGame.BuildingInfoDisplayer;
using UnityEngine;

namespace BuildingManage
{

    public class BuildingSelectController : MonoBehaviour
    {
        [SerializeField] private UIInputReaderSO _uiInputReader;
        [SerializeField] private BuildingInfoDisplayPanel _buildingInfoPanel;
        private ISelectable _currentSelect;
        private void Awake()
        {
            _uiInputReader.SelectEvent += HandleSelectBuilding;
        }

        private void OnDestroy()
        {
            _uiInputReader.SelectEvent -= HandleSelectBuilding;
        }

        public void HandleSelectBuilding(ISelectable selectedBuilding)
        {
            if (_currentSelect != null)
                _currentSelect.UnSelect();
            _currentSelect = selectedBuilding;
            if (selectedBuilding == null) 
            {
                _buildingInfoPanel.Close();
                return;
            }
            _buildingInfoPanel.Open();
            _buildingInfoPanel.SelectBuilding(selectedBuilding.GetInformation());
        }


    }

}