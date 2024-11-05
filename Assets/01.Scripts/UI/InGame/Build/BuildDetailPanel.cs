using BuildingManage;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Build
{
    public class BuildDetailPanel : UIPanel
    {
        [SerializeField] private float _defaultYPos;
        [SerializeField] private float _activeYPos;
        
        [Header("Essential Settings")]
        [SerializeField] private Image _buildingIcon;
        [SerializeField] private TextMeshProUGUI _buildingNameText;
        [SerializeField] private BuildMaterialPanel _buildMaterialPanel;
        private FabricSO _buildingInfo;

        private RectTransform _rectTrm;

        protected override void Awake()
        {
            base.Awake();
            _rectTrm = transform as RectTransform;
        }

        public override void Open()
        {
            base.Open();
            _rectTrm.DOAnchorPosY(_activeYPos, _activeDuration);
        }

        public override void Close()
        {
            base.Close();
            _rectTrm.DOAnchorPosY(_defaultYPos, _activeDuration);
        }

        public void HandleSettingBuildDetail(FabricSO buildingInfo)
        {
            _buildingInfo = buildingInfo;
            _buildingNameText.text = buildingInfo.buildingName;
            _buildMaterialPanel.HandleSetMaterialSlots(_buildingInfo.needResource);    
            
        }
        
    }
}