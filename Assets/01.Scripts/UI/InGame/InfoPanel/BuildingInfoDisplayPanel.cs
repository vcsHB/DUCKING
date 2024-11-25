using BuildingManage.Tower;
using UnityEngine;

namespace UI.InGame.BuildingInfoDisplayer
{
    public class BuildingInfoDisplayPanel : MovePanel
    {
        private Building _currentBuilding;
        [SerializeField] private BuildingStatusTitlePanel _buildingStatusPanel;
        [SerializeField] private FactoryBuildingDetailInfoPanel _factoryBuildingDetailInfoPanel;
        [SerializeField] private TowerBuildingDetailInfoPanel _towerBuildingDetailInfoPanel;

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
            
            _factoryBuildingDetailInfoPanel.Close();
            _towerBuildingDetailInfoPanel.Close();
            if(building is Factory)
            {
                
                _factoryBuildingDetailInfoPanel.Initialize(building as Factory);
            }        
            if(building is Tower)
            {
                _towerBuildingDetailInfoPanel.Initialize(building as Tower);
            }

        }
    }
}