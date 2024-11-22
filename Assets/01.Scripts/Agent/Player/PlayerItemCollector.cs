using System;
using System.Collections.Generic;
using ItemSystem;
using UnityEngine;
using UnityEngine.Events;

namespace AgentManage.PlayerManage
{   
    public class PlayerItemCollector : MonoBehaviour, IAgentComponent, IItemCollectable
    {
        [SerializeField] private ItemInfoGroupSO itemInfoGroupSo;
        public event Action OnItemValueChange;

        [SerializeField] private List<ItemData> _inventory = new List<ItemData>(10);
        public List<ItemData> Inventory => _inventory;
        [field:SerializeField] public int InventorySize { get; private set; } = 10;


        private Player _player;
        private Dictionary<int, int> _itemSortingTable = new Dictionary<int, int>();
        
        private void Awake()
        {
            //print(InventorySize + "설정된 인벤사이즈");

            //_inventory = new List<ItemData>(1);

            // 처음에 로드 한번 하고 해줘야됨
            RefreshEmptySlot();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //print("밍");
                RemoveItem(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                RemoveItem(1);
            }
        }

        #region AgentCompo Func

        
        public void Initialize(Agent agent)
        {
            _player = agent as Player;


        }

        public void AfterInit()
        {

        }

        public void Dispose()
        {

        }


        #endregion


        #region external Change Funcs


        public int CollectItem(int id)
        {
            return CollectItem(id, 1);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemData"></param>
        /// <returns>
        /// 다 채우고 남은 아이템 개수
        /// </returns>
        public int CollectItem(int id, int amount)
        {
            // 1. 인벤에 공간이 충분한지 체크
            // 2. 아이템 뭉쳐지는 크기 체크
            //    크기별로 분할했을때 슬롯 량이 충분한지 체크

            RefreshEmptySlot();
            ItemInfoSO itemInfoSo = itemInfoGroupSo.GetItemData(id);
            int currentInsertAmount = amount;
            while (currentInsertAmount > 0)
            {
                ItemData slot = GetNotFullSlot(id, itemInfoSo.itemMaxGroupingAmount);
                if (slot == null || currentInsertAmount <= 0) return currentInsertAmount;
                
                int leftSize = itemInfoSo.itemMaxGroupingAmount - slot.amount;
                //print($"남은 용량 : {leftSize}, 추가할 내용량 : {currentInsertAmount}");

                if (currentInsertAmount > leftSize)
                {
                    currentInsertAmount -= leftSize;
                    slot.amount += leftSize;
                    
                }else
                {
                    slot.amount += currentInsertAmount;
                    currentInsertAmount = 0;
                    break;
                }
            }
            return 0;
        }

        /// <summary>
        /// 덜 찬 슬롯을 찾아 가져오는 함수
        /// </summary>
        /// <param name="id">덜 찬 아이템</param>
        /// <param name="max">그 아이템의 최대 그루핑 개수</param>
        /// <returns>덜 찬 슬롯</returns>
        private ItemData GetNotFullSlot(int id, int max)
        {
            // id와 max값을 받아 id에 해당하는 슬롯의 amount값이 max값을 초과하지 않는 슬롯을 반환
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].id == id && _inventory[i].amount < max)
                    return _inventory[i];
            }
            ItemData emptySlot = GetEmptySlot();
            if (emptySlot != null) // 만만한 빈 슬롯이 하나 있으면 
            {
                emptySlot.id = id;
                return emptySlot;
            }
            // 다 꽉찬 슬롯인.
            return null;
        }
        
        public void RemoveItem(int id)
        {
            RemoveItem(new ItemData { id = id, amount = 1 }); // 이게 맞다는 사실. 
            // 디버그 코드라 걍 쓴.
        }

        public void RemoveItem(ItemData itemData)
        {
            // 1. 인벤에 일단 존재는 하는지 체크
            // 2. 뺄수 있는 양인지 체크 (GetItemAmount 활용)
            // 3. 빼기
            RefreshEmptySlot();
            if (GetItemAmount(itemData.id) < itemData.amount)
            {
                return; // 뺄 수 없음
            }
            print("뺄수 있는지 체크 다함.");
            int currentMinus = 0;

            while (currentMinus < itemData.amount)
            {
                ItemData slot = GetItemSlot(itemData.id);
                int minus = itemData.amount - currentMinus;
                if (slot.amount > minus) // 남은 양이 뺄 양보다 많으면
                {
                    slot.amount -= minus;
                    currentMinus += minus;
                }
                else
                {
                    currentMinus += slot.amount;
                    slot.amount = 0;
                }
            }
        }

        #endregion

        private void RefreshEmptySlot()
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].amount <= 0)
                {
                    _inventory[i].id = -1;
                    _inventory[i].amount = 0;
                }
            }
        }

        public void HandleInventorySort()
        {
            _itemSortingTable.Clear();
            foreach (ItemData itemData in _inventory)
            {
                if (!_itemSortingTable.ContainsKey(itemData.id))
                {
                    _itemSortingTable.Add(itemData.id, 0);
                }
                _itemSortingTable[itemData.id] += itemData.amount;
                itemData.amount = 0;
            }
            RefreshEmptySlot();
            foreach (var pair in _itemSortingTable)
            {
                CollectItem(pair.Key, pair.Value);
            }
        }
        
        #region external Check Funcs

        
        
        public bool CheckEmptySlot()
        {
            int emptySlotAmount = 0;
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].amount <= 0)
                {
                    _inventory[i].id = -1;
                    _inventory[i].amount = 0;
                    emptySlotAmount++;
                }
            }
            return emptySlotAmount > 0;
        }

        public int GetEmptySlotAmount()
        {
            int emptySlotAmount = 0;
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].amount <= 0)
                {
                    _inventory[i].id = -1;
                    _inventory[i].amount = 0;
                    emptySlotAmount++;
                }
            }
            return emptySlotAmount;
        }

        private ItemData GetEmptySlot()
        {
            RefreshEmptySlot();
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].amount == 0)
                {
                    return _inventory[i];
                }
            }
            
            // 빈게 없음.
            return null;
        }

        private ItemData GetItemSlot(int id)
        {
            RefreshEmptySlot();
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].id == id)
                {
                    return _inventory[i];
                }
            }
            return null;
        }
        
        /**
         * <summary>
         * 특정 아이템을 가지고 있는지 체크
         * </summary>
         */
        public bool IsHaveItem(int id)
        {
            foreach (ItemData item in _inventory)
            {
                if (item.id == id)
                    return true;
            }
            return false;
        }
        
        /**
         * <summary>
         * 특정 아이템의 개수를 반환
         * </summary>
         */
        public int GetItemAmount(int id)
        {
            int total = 0;
            foreach (ItemData item in _inventory)
            {
                if (item.id == id)
                    total += item.amount;
            }
            return total;
        }        

        #endregion

        #region Debug Func

        
        #endregion


    }
}