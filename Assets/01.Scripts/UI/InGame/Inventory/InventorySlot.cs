using ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.InGame.Inventory
{
    
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _amountText;
        [FormerlySerializedAs("_currentItem")]
        [Header("Current Information")]
        [SerializeField] private ItemInfoSO currentItemInfo;
        [SerializeField] private int amount;
        
        
        public void Initialize(ItemInfoSO itemInfoSo, int _amount)
        {
            // 나중에 ItemSO를 넘겨받아서 스프라이트 데이터 적용
            _amount = amount;
            currentItemInfo = itemInfoSo;
            _iconImage.sprite = itemInfoSo.itemSprite;
            
            if (itemInfoSo.id == -1)
            {
                _amountText.text = string.Empty;
                return;
            }
            _amountText.text = _amount.ToString();
        }

    }

}