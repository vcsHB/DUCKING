using System.Collections.Generic;
using AgentManage.PlayerManage;
using InputManage;
using ItemSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.InGame.Inventory
{
    public class InventoryPanel : UIPanel
    {
        [Header("Setting")]
        [SerializeField] private UIInputReaderSO _uiInputReader;
        [SerializeField] private PlayerItemCollector _playerItemCollector;
        [FormerlySerializedAs("_itemDataGroupSO")] [SerializeField] private ItemInfoGroupSO itemInfoGroupSo;
        [SerializeField] private InventorySlot _slotPrefab;
        [SerializeField] private RectTransform _contentTrm;
        // 정말 단순히 보여주기만 하는 기능만을 가지고 있어야 함
        private List<InventorySlot> _slots = new List<InventorySlot>();



        protected override void Awake()
        {
            base.Awake();
            _uiInputReader.OnInventoryOpenEvent += HandleOpenInventory;
            _uiInputReader.OnInventorySortEvent += HandleSortInventory;
        }

        private void HandleOpenInventory()
        {
            if (!_isActive)
            {
                OpenInventory(_playerItemCollector.Inventory);
            }
            else
            {
                Close();
            }
        }

        public void OpenInventory(List<ItemData> inventoryInfo)
        {
            Open();
            print(inventoryInfo.Count +" 인벤 사이즈");
            RefreshInventory(inventoryInfo);
        }

        public void HandleSortInventory()
        {
            _playerItemCollector.HandleInventorySort();
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
                }
            }
            
            for (int i = 0; i < size; i++)
            {
                ItemData itemData = inventoryInfo[i];
                InventorySlot slot = _slots[i];
                print("id: "+itemData.id + " amount : "+itemData.amount);
                slot.Initialize(
                    itemInfoGroupSo.GetItemData(itemData.id),
                    itemData.amount
                    );
            }
        }

    }
}