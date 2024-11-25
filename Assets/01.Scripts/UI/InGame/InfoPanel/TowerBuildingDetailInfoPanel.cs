using System;
using BuildingManage.Tower;
using ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI.InGame.BuildingInfoDisplayer
{
    public class TowerBuildingDetailInfoPanel : UIPanel
    {
        [SerializeField] private ItemInfoGroupSO _itemInfoGroup;
        [SerializeField] private Image _ammoResourceImage;
        [SerializeField] private Image _ammoGaugeImage;
        [SerializeField] private TextMeshProUGUI _ammoAmountText;

        private Tower _owner;

        public void Initialize(Tower tower)
        {
            Open();
            if (_owner != null)
            {
                tower.OnResourceChangedEvent -= HandleResourceChanged;
                tower.OnDestroyEvent -= Close;
                // 구독 해제 작업
            }
            _owner = tower;
            ItemInfoSO itemInfo = _itemInfoGroup.GetItemData((int)(tower.NeedResource.type));
            _ammoResourceImage.sprite = itemInfo.itemSprite;
            tower.OnResourceChangedEvent += HandleResourceChanged;
            tower.OnDestroyEvent += Close;
            HandleResourceChanged(tower.CurrentResourceAmount, tower.MaxResourceAmount);

        }

        private void HandleResourceChanged(int current, int max)
        {
            _ammoGaugeImage.fillAmount = (float)current / max;
            _ammoAmountText.text = $"{current}/{max}";

        }
    }
}