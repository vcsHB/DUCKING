using System;
using System.Collections.Generic;
using ItemSystem;
using UI.InGame.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace AgentManage.Player
{
    public class PlayerItemCollector : MonoBehaviour
    {
        [SerializeField] private ItemDataGroupSO _itemDataGroupSO;
        public UnityEvent OnItemCollectEvent;
        [SerializeField] private InventoryPanel _inventoryUI;

        private List<ItemData> _inventory;
        public int InventorySize { get; private set; } = 10;


        private void Awake()
        {
            _inventory = new List<ItemData>(InventorySize);
        }


        #region external Change Funcs


        public void CollectItem(int id)
        {
            CollectItem(new ItemData {id = id, amount = 1});
        }
        
        public void CollectItem(ItemData itemData)
        {
            // 1. 인벤에 공간이 충분한지 체크
            // 2. 아이템 뭉쳐지는 크기 체크
            //    크기별로 분할해서 저장
            //    
            // 
            
        }

        public void RemoveItem(int id)
        {
            RemoveItem(new ItemData { id = id, amount = 1 });
        }

        public void RemoveItem(ItemData itemData)
        {
            // 1. 인벤에 일단 존재는 하는지 체크
            // 2. 뺄수 있는 양인지 체크 (GetItemAmount 활용)
            // 3. 빼기
        }

        #endregion

        #region external Check Funcs

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
        
    }
}