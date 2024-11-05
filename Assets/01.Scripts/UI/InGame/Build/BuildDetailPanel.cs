using BuildingManage;
using UnityEngine;

namespace UI.InGame.Build
{
    public class BuildDetailPanel : UIPanel
    {
        [SerializeField] private BuildMaterialPanel _buildMaterialPanel;
        private FabricSO _buildingInfo;
        
        public void HandleSettingBuildDetail(FabricSO buildingInfo)
        {
            _buildingInfo = buildingInfo;
            _buildMaterialPanel.HandleSetMaterialSlots(_buildingInfo.needResource);    
            
        }
        
    }
}