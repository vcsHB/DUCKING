using UnityEngine;
namespace UI.InGame.BuildingInfoDisplayer
{

    public class BuildingInfoDisplayPanel : MovePanel
    {
        private Building _currentBuilding;
        [SerializeField] private BuildingStatusTitlePanel _buildingStatusPanel;

        public void SelectBuilding(Building building)
        {
            if(_currentBuilding != null)
            {
                _currentBuilding.HealthCompo.OnHealthChangeEvent -= _buildingStatusPanel.HandleRefreshHealthGauge;
            }

            _currentBuilding = building;
            _currentBuilding.HealthCompo.OnHealthChangeEvent += _buildingStatusPanel.HandleRefreshHealthGauge;
            _buildingStatusPanel.HandleRefreshHealthGauge(_currentBuilding.HealthCompo.CurrentHealth, _currentBuilding.HealthCompo.MaxHealth);
            _buildingStatusPanel.SetBuildingInfo(building.BuildingInfo.buildingName, building.BuildingInfo.buildingIconSprite);
            

        }
    }
}