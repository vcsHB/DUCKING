using System;
using ObjectPooling;
using UnityEngine;

namespace AgentManage.Enemys
{

    public class Enemy : Agent, IPoolable
    {
        [field:SerializeField] public PoolingType type { get; set; }
        public Action<Enemy> OnEnemyDieEvent;

        public GameObject ObjectPrefab => gameObject;

        public void ResetItem()
        {
            HealthCompo.ResetHealth();
        }

        protected override void Awake()
        {
            base.Awake();
            HealthCompo.OnDieEvent.AddListener(HandleDie);
        }

        private void HandleDie()
        {
            OnEnemyDieEvent?.Invoke(this);
            PoolManager.Instance.Push(this);
            
        }


    }

}