using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class ItemDropManager : MonoBehaviour
    {
        [SerializeField] private DropItem _dropItemPrefab;
        [SerializeField] private Queue<DropItem> dropItemPool;

        public void GenerateDropItem(int id, int amount)
        {
            
        }
    }
}