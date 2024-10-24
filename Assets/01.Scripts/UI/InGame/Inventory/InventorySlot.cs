using ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory
{
    
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _amountText;
        [Header("Current Information")]
        [SerializeField] private ItemSO _currentItem;
        [SerializeField] private int amount;
        
        
        public void Initialize(ItemSO itemSO, int _amount)
        {
            // 나중에 ItemSO를 넘겨받아서 스프라이트 데이터 적용
            _amount = amount;
            _currentItem = itemSO;
            _iconImage.sprite = itemSO.itemSprite;
            
            if (itemSO.id == -1)
            {
                _amountText.text = string.Empty;
                return;
            }
            _amountText.text = _amount.ToString();
        }

    }

}