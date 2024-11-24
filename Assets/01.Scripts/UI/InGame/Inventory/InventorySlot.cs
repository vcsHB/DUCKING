using System;
using ItemSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _amountText;
        [Header("Current Information")]
        [SerializeField] private ItemInfoSO _currentItemInfo;
        internal ItemInfoSO CurrentItemInfo => _currentItemInfo;
        [SerializeField] private int _amount;
        internal int Amount => _amount;
        public event Action<InventorySlot> OnItemSelectEvent;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleItemSelect);
        }

        private void HandleItemSelect()
        {
            if(_amount <= 0) return;

            OnItemSelectEvent?.Invoke(this);
        }

        public void Initialize(ItemInfoSO itemInfoSo, int amount)
        {
            // 나중에 ItemSO를 넘겨받아서 스프라이트 데이터 적용
            _amount = amount;
            _currentItemInfo = itemInfoSo;

            if (itemInfoSo.id == -1)
            {
                _iconImage.enabled = false;
                _amountText.text = string.Empty;
                return;
            }
            _iconImage.enabled = true;
            _iconImage.sprite = itemInfoSo.itemSprite;
            _amountText.text = _amount.ToString();
        }

    }

}