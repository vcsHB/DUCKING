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
        
        
        public void Initialize()
        {
            // 나중에 ItemSO를 넘겨받아서 스프라이트 데이터 적용
        }

    }

}