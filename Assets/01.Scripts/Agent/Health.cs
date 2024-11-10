using System;
using Combat;
using UnityEngine;
using UnityEngine.Events;

namespace AgentManage
{
    public class Health : MonoBehaviour, IDamageable, IHealable
    {
        [Header("Events")] 
        public UnityEvent OnHealthDecreaseEvent;
        public UnityEvent OnHealthInCreaseEvent;
        public UnityEvent OnDieEvent;
        public event Action<int, int> OnHealthChangeEvent; 
        
        [SerializeField] private int _currentHealth;
        public int CurrentHealth => _currentHealth;
        public int MaxHealth { get; protected set; }

        internal void SetMaxHealth(int max)
        {
            MaxHealth = max;
        }

        internal void ResetHealth()
        {
            _currentHealth = MaxHealth;
        }

        internal void FillHealth(float percent)
        {
            _currentHealth += (int)(MaxHealth * percent);
            OnHealthInCreaseEvent?.Invoke();
        }
        
        
        public virtual void ApplyDamage(int amount)
        {
            _currentHealth -= amount;
            OnHealthDecreaseEvent?.Invoke();
            InvokeHealthChange();
            CheckDie();
        }

        public virtual void RestoreHealth(int amount)
        {
            _currentHealth += amount;
            OnHealthInCreaseEvent?.Invoke();
            InvokeHealthChange();
            ClampHealth();
        }

        private void CheckDie()
        {
            if (_currentHealth <= 0)
            {
                OnDieEvent?.Invoke();
            }
        }

        protected void ClampHealth()
        {
            _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);
        }

        private void InvokeHealthChange()
        {
            OnHealthChangeEvent?.Invoke(_currentHealth, MaxHealth);
        }
    }

}