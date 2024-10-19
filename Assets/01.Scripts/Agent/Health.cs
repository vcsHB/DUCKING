using System;
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
        
        [SerializeField] internal int _currentHealth;
        public int MaxHealth { get; private set; }

        internal void SetMaxHealth(int max)
        {
            MaxHealth = max;
        }
        
        
        public void ApplyDamage(int amount)
        {
            _currentHealth -= amount;
            OnHealthDecreaseEvent?.Invoke();
            InvokeHealthChange();
            CheckDie();
        }

        public void RestoreHealth(int amount)
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

        private void ClampHealth()
        {
            _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);
        }

        private void InvokeHealthChange()
        {
            OnHealthChangeEvent?.Invoke(_currentHealth, MaxHealth);
        }
    }

}