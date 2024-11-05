using BuildingManage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Build
{
    public class BuildDetailPanel : UIPanel
    {
        [SerializeField] private Image _buildingIcon;
        [SerializeField] private TextMeshProUGUI _buildingNameText;
        [SerializeField] private BuildMaterialPanel _buildMaterialPanel;
        private FabricSO _buildingInfo;
        
        public void HandleSettingBuildDetail(FabricSO buildingInfo)
        {
            _buildingInfo = buildingInfo;
            _buildingNameText.text = buildingInfo.buildingName;
            _buildMaterialPanel.HandleSetMaterialSlots(_buildingInfo.needResource);    
            
        }
        
    }
}