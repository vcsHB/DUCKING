using System;
using ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI.InGame.Inventory
{

    public class ItemInfoPanel : UIPanel
    {
        public event Action<int> ItemDecreaseEvent;

        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private Vector2 _offset;

        [SerializeField] private Button _useButton;
        [SerializeField] private Button _dropButton;
        private ItemInfoSO _currentInfo;
        private RectTransform _rectTrm;
        private InventorySlot _currentSlot;

        protected override void Awake()
        {
            base.Awake();
            _rectTrm = transform as RectTransform;

            _useButton.onClick.AddListener(HandleUse);
            _dropButton.onClick.AddListener(HandleDrop);
        }


        public void SetItemInfo(InventorySlot slot, Vector2 position)
        {
            _currentSlot = slot;
            _rectTrm.anchoredPosition = position + _offset;
            _currentInfo = slot.CurrentItemInfo;
            _descriptionText.text = _currentInfo.description;
            _amountText.text = $"{slot.Amount.ToString()}개";
            UsableItemInfoSO usableItemInfo = _currentInfo as UsableItemInfoSO;
            bool isUsable = usableItemInfo != null;
            _useButton.gameObject.SetActive(isUsable);
            if (isUsable)
            {
            }
            else
            {

            }

        }

        private void HandleUse()
        {
            UsableItemInfoSO usableItemInfo = _currentInfo as UsableItemInfoSO;
            ItemDropManager.Instance.GenerateUsableItem(usableItemInfo.itemPrefab);
            ItemDecreaseEvent?.Invoke(1);
        }


        private void HandleDrop()
        {
            ItemDropManager.Instance.GenerateDropItem(_currentInfo.id, _currentSlot.Amount);
            ItemDecreaseEvent?.Invoke(_currentSlot.Amount);
        }
    }

}