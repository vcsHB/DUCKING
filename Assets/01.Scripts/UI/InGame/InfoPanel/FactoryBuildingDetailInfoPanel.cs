using ItemSystem;
using TMPro;
using UI.InGame.BuildingInfoDisplayer.FactoryDetail;
using UnityEngine;
using UnityEngine.UI;
namespace UI.InGame.BuildingInfoDisplayer
{

    public class FactoryBuildingDetailInfoPanel : UIPanel
    {

        [SerializeField] private Image _progressBarImage;
        [SerializeField] private Image _outputResourceImage;
        [SerializeField] private TextMeshProUGUI _outputAmountText;
        [SerializeField] private ItemInfoGroupSO _itemInfoGroupSO;
        [SerializeField] private TextMeshProUGUI _progressAndLeftTimeText;
        [SerializeField] private FactoryBuildingRequireMaterialSlot[] _requireMaterialSlots;
        private Factory _owner;

        public void Initialize(Factory factory)
        {
            if (_owner != null)
            {
                _owner.OnProgressEvent -= HandleProgress;
                _owner.OnStorageChanged -= HandleStorageChanged;
                _owner.OnDestroyEvent -= Close;
            }
            Open();
            _owner = factory;
            _outputResourceImage.sprite = _itemInfoGroupSO.GetItemData((int)(_owner.OutPut[0].type)).itemSprite;
            _outputAmountText.text = _owner.OutPut[0].amount.ToString();
            _owner.OnProgressEvent += HandleProgress;
            _owner.OnStorageChanged += HandleStorageChanged;
            _owner.OnDestroyEvent += Close;
            // for(int i = 0; i < _owner.RequireResources.Keys.Count; i++)
            // {
            //     HandleStorageChanged()

            // }
            HandleStorageChanged();

        }

        private void HandleStorageChanged()
        {
            // var resources = _owner.Storage;
            for (int i = 0; i < _requireMaterialSlots.Length; i++)
            {
                _requireMaterialSlots[i].SetEnable(false);
            }
            int index = 0;
            foreach (var slot in _owner.Storage)
            {
                ItemInfoSO itemInfo = _itemInfoGroupSO.GetItemData((int)(slot.Key));
                _requireMaterialSlots[index].HandleRefreshSlot(itemInfo.itemSprite, slot.Value, _owner.RequireResources[slot.Key]);
                index++;
            }

        }




        public void HandleProgress(float currentTime, float processTime)
        {
            float ratio = Mathf.Clamp01(currentTime / processTime);
            _progressBarImage.fillAmount = ratio;
            _progressAndLeftTimeText.text = $"진행도 ({(int)(ratio * 100)}%) 남은 시간 ({processTime - currentTime}s)";

        }



    }
}