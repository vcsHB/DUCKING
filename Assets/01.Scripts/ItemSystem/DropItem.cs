using System;
using ObjectPooling;
using UnityEngine;

namespace ItemSystem
{
    public class DropItem : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public PoolingType type { get; set; }
        public GameObject ObjectPrefab => gameObject;
        public void ResetItem()
        {
        }
        
        [SerializeField] private ItemData _itemData;

        [Header("Setting Values")]
        [SerializeField] private float _detectRadius = 0.4f;

        [SerializeField] private LayerMask _detectMask;
        
        private SpriteRenderer _spriteRenderer;
        private ItemInfoSO _itemInfo;
        
        private void Awake()
        {
            _spriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        }

        public void Initialize(int id, int amount, ItemInfoSO itemInfo)
        {
            _itemData.id = id;
            _itemData.amount = amount; 
            _spriteRenderer.sprite = itemInfo.itemSprite;
        }

        private void Update()
        {
            CheckCollector();
        }


        private void CheckCollector()
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, _detectRadius, _detectMask);
            if (hit == null) return;
            
            if (hit.transform.TryGetComponent(out IItemCollectable collectable))
            {
                collectable.CollectItem(_itemData.id, _itemData.amount);
                PoolManager.Instance.Push(this);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _detectRadius);
        }

        
    }
}