using System;
using ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Build
{
    public class BuildMaterialSlot : MonoBehaviour
    {
        private int _needAmount;
        private ItemInfoSO _itemInfo;
        [SerializeField] private Image _materialImage;
        [SerializeField] private TextMeshProUGUI _materialAmountText;


        private Color _defaultColor, _shortageColor = Color.red;

        public ItemInfoSO ItemInfo => _itemInfo;

        private void Awake()
        {
            _defaultColor = _materialAmountText.color;
        }

        public void UpdateHaveAmount(int amount)
        {
            bool isEnough = amount >= _needAmount;
            _materialAmountText.text = $"{amount}/{_needAmount}";

            _materialAmountText.color = isEnough ? _defaultColor : _shortageColor;
        }

        public void SetMaterial(ItemInfoSO materialInfo, int needAmount, int haveAmount)
        {
            _needAmount = needAmount;
            bool isEnough = haveAmount >= needAmount;

            _itemInfo = materialInfo;
            gameObject.SetActive(true);
            _materialAmountText.color = isEnough ? _defaultColor : _shortageColor;
            _materialAmountText.text = $"{haveAmount}/{needAmount}";
            _materialImage.sprite = _itemInfo.itemSprite;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
        
    }
}