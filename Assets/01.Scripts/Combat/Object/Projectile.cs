using System;
using System.Collections;
using ObjectPooling;
using UnityEngine;

namespace Combat
{
    
    public class Projectile : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public PoolingType type { get; set; }
        public GameObject ObjectPrefab => gameObject;

        [SerializeField] private float _lifeTime;
        [SerializeField] private float _speed = 4f;
        [SerializeField] private PoolingType _projectileDestroyVFX;
        private DamageCaster _damageCaster;
        private float _currentTime = 0;
        private Vector2 _direction;
        
        private void Awake()
        {
            _damageCaster = GetComponent<DamageCaster>();
            _damageCaster.OnCastEvent += Destroy;
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;
            transform.
            transform.position += (Vector3)(_direction * _speed * Time.deltaTime);
            if (_currentTime >= _lifeTime)
            {
                _currentTime = 0;
                Destroy();
            }
        }

        private void FixedUpdate()
        {
            _damageCaster.CastDamage();
        }

        public void Fire(Vector2 direction)
        {
            _direction = direction.normalized;
            transform.right = _direction;
            
        }


        private void Destroy()
        {
            PoolManager.Instance.Push(this);
        }

        public void ResetItem()
        {
            
        }
        
    }

}