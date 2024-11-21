using System;
using ObjectPooling;
using UnityEngine;

namespace AgentManage.Enemies
{

    public class Enemy : Agent, IPoolable
    {
        [field: SerializeField] public PoolingType type { get; set; }
        public Action<Enemy> OnEnemyDieEvent;

        public GameObject ObjectPrefab => gameObject;

        public Transform target { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            HealthCompo.OnDieEvent.AddListener(HandleDie);
        }

        public void ResetItem()
        {
            IsDead = false;
            HealthCompo.ResetHealth();
        }

        public void SetTartget(Transform target)
        {
            this.target = target;
        }

        public virtual void Initialize() { }

        private void HandleDie()
        {
            OnEnemyDieEvent?.Invoke(this);
            PoolManager.Instance.Push(this);
        }
    }

}