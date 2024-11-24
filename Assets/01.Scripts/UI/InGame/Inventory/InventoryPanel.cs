using System;
using System.Collections.Generic;
using AgentManage.PlayerManage;
using InputManage;
using ItemSystem;
using UnityEngine;

namespace UI.InGame.Inventory
{
    public class InventoryPanel : UIPanel
    {
        [Header("Setting")]
        [SerializeField] private UIInputReaderSO _uiInputReader;
        [SerializeField] private PlayerItemCollector _playerItemCollector;
        [SerializeField] private ItemInfoPanel _itemInfoPanel;
        [SerializeField] private ItemInfoGroupSO itemInfoGroupSo;
        [SerializeField] private InventorySlot _slotPrefab;
        [SerializeField] private RectTransform _contentTrm;
        // 정말 단순히 보여주기만 하는 기능만을 가지고 있어야 함
        private List<InventorySlot> _slots = new List<InventorySlot>();
        private InventorySlot _selectedSlot;

        protected override void Awake()
        {
            base.Awake();
            _uiInputReader.OnInventoryOpenEvent += HandleToggleInventory;
            _uiInputReader.OnInventorySortEvent += HandleSortInventory;
            _playerItemCollector.OnItemValueChange += HandleInventoryChanged;
            _itemInfoPanel.ItemDecreaseEvent += HandleDecreaseSelectedItem;
        }

        public void HandleToggleInventory()
        {
            if (!_isActive)
            {
                OpenInventory(_playerItemCollector.Inventory);
                Time.timeScale = 0;
            }
            else
            {
                Close();
                Time.timeScale = 1;
            }
        }

        public void OpenInventory(List<ItemData> inventoryInfo)
        {
            Open();
            print(inventoryInfo.Count + " 인벤 사이즈");
            RefreshInventory(inventoryInfo);
        }

        public void HandleSortInventory()
        {
            _playerItemCollector.HandleInventorySort();
            RefreshInventory(_playerItemCollector.Inventory);
        }

        private void HandleInventoryChanged()
        {
            if (!_isActive) return;
            RefreshInventory(_playerItemCollector.Inventory);
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
                    slot.OnItemSelectEvent += HandleItemSelect;
                }
            }

            for (int i = 0; i < size; i++)
            {
                ItemData itemData = inventoryInfo[i];
                InventorySlot slot = _slots[i];
                //print("id: " + itemData.id + " amount : " + itemData.amount);
                slot.Initialize(
                    itemInfoGroupSo.GetItemData(itemData.id),
                    itemData.amount
                    );
            }
        }

        private void HandleItemSelect(InventorySlot slot)
        {
            _selectedSlot = slot;
            _itemInfoPanel.SetItemInfo(slot, (slot.transform as RectTransform).anchoredPosition);
        }

        private void HandleDecreaseSelectedItem(int amount)
        {
            _playerItemCollector.RemoveItem(_selectedSlot.CurrentItemInfo.id, amount);
            HandleSortInventory();
        }
    }
}