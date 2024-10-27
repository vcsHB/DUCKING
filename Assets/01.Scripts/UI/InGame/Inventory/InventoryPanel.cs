using System.Collections.Generic;
using ItemSystem;
using UnityEngine;

namespace UI.InGame.Inventory
{
    public class InventoryPanel : UIPanel
    {
        [SerializeField] private ItemDataGroupSO _itemDataGroupSO;
        [SerializeField] private InventorySlot _slotPrefab;
        [SerializeField] private RectTransform _contentTrm;
        // 정말 단순히 보여주기만 하는 기능만을 가지고 있어야 함
        private List<InventorySlot> _slots = new List<InventorySlot>();


        public void OpenInventory(List<ItemData> inventoryInfo)
        {
            Open();
            RefreshInventory(inventoryInfo);
        }

        public void RefreshInventory(List<ItemData> inventoryInfo)
        {
            int size = inventoryInfo.Count;
            if (_slots.Count < size)
            {
                int shortageAmount = (size - _slots.Count); 
                for (int i = 0; i < shortageAmount; i++)
                {
                    InventorySlot slot = Instantiate(_slotPrefab, _contentTrm);
                    _slots.Add(slot);
                }
            }

            for (int i = 0; i < size; i++)
            {
                ItemData itemData = inventoryInfo[i];
                InventorySlot slot = _slots[i];
                
                slot.Initialize(
                    _itemDataGroupSO.GetItemData(itemData.id), 
                    itemData.amount
                    );
            }
        }
        
    }
}