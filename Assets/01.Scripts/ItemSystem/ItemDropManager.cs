using System;
using System.Collections.Generic;
using ObjectPooling;
using Objects.UsableItem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ItemSystem
{
    public class ItemDropManager : MonoSingleton<ItemDropManager>
    {
        [SerializeField] private Transform _playerTrm;
        [SerializeField] private ItemInfoGroupSO _itemInfoGroup;
        [SerializeField] private DropItem _dropItemPrefab;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DebugSpawnItem(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                DebugSpawnItem(1);
            }
        }

        private void DebugSpawnItem(int id)
        {
            Vector2 position = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            GenerateDropItem(id, 1, position);
        }
        
        public void GenerateDropItem(int id, int amount, Vector2 position)
        {
            DropItem dropItem = PoolManager.Instance.Pop(PoolingType.DropItem, position, Quaternion.identity) as DropItem;
            dropItem.Initialize(id, amount, _itemInfoGroup.GetItemData(id));
        }


        public void GenerateUsableItem(UsableItem itemPrefab)
        {
            UsableItem item = Instantiate(itemPrefab);
            item.Use(_playerTrm.position);
        }
    }
}