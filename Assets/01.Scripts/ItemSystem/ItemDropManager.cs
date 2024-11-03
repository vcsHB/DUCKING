using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class ItemDropManager : MonoBehaviour
    {
        [SerializeField] private ItemInfoGroupSO _itemInfoGroup;
        [SerializeField] private DropItem _dropItemPrefab;
        [SerializeField] private Queue<DropItem> dropItemPool;


        [ContextMenu("DebugGenItem")]
        private void DebugSpawnItem()
        {
            Vector2 position = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            GenerateDropItem(0, 1, position);
        }
        
        public void GenerateDropItem(int id, int amount, Vector2 position)
        {
            DropItem dropItem = Instantiate(_dropItemPrefab, position, Quaternion.identity);
            dropItem.Initialize(id, amount, _itemInfoGroup.GetItemData(id));
        }
    }
}