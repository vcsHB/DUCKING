using System;
using ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Build
{
    public class BuildMaterialSlot : MonoBehaviour
    {
        [SerializeField] private Image _materialImage;
        [SerializeField] private TextMeshProUGUI _materialAmountText;

        private Color _defaultColor, _shortageColor = Color.red;

        private void Awake()
        {
            _defaultColor = _materialAmountText.color;
        }

        public void SetMaterial(ItemInfoSO materialInfo, int needAmount, int haveAmount)
        {
            bool isEnough = haveAmount >= needAmount;
            gameObject.SetActive(true);
            _materialAmountText.color = isEnough ? _defaultColor : _shortageColor;
            _materialAmountText.text = $"{haveAmount}/{needAmount}";
            _materialImage.sprite = materialInfo.itemSprite;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
        
    }
}