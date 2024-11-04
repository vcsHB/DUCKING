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

        private Color _defaultColor, _lackColor = Color.red;

        private void Awake()
        {
            _defaultColor = _materialAmountText.color;
        }

        public void SetMaterial(ItemInfoSO materialInfo, bool isEnough)
        {
            _materialAmountText.color = isEnough ? _defaultColor : _lackColor;
            _materialAmountText.text = materialInfo.itemName;
            _materialImage.sprite = materialInfo.itemSprite;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
        
    }
}