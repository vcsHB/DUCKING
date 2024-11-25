using System;
using AgentManage;
using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    
    public class DamageCaster : MonoBehaviour
    {
        public event Action OnCastEvent;
        public UnityEvent OnCastUnityEvent;
        [SerializeField] protected LayerMask _targetLayer;
        [SerializeField] protected float _detectRange;
        [SerializeField] protected int _damage;
        [SerializeField] protected int _maxTargetAmount;

        private Collider2D[] _hits;
        private IAdditionalCaster[] _casters;
        
        private void Awake()
        {
            _hits = new Collider2D[_maxTargetAmount];
            _casters = GetComponents<IAdditionalCaster>();
        }

        public void CastDamage()
        {
            int amount = Physics2D.OverlapCircleNonAlloc(transform.position, _detectRange, _hits, _targetLayer);

            for (int i = 0; i < amount; i++)
            {
                if (_hits[i].TryGetComponent(out IDamageable hit))
                {
                    hit.ApplyDamage(_damage);
                    CastAllCasters(_hits[i]);
                }
                OnCastEvent?.Invoke();
                OnCastUnityEvent?.Invoke();
            }
        }

        private void CastAllCasters(Collider2D target)
        {
            foreach (var caster in _casters)
            {
                caster.Cast(target);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _detectRange);
        }
    }

}