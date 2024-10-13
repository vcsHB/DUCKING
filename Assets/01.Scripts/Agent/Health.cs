using System.Collections;
using System.Collections.Generic;
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
        
        [SerializeField] internal int _currentHealth;
        public int MaxHealth { get; private set; }

        public void ApplyDamage(int amount)
        {
            _currentHealth -= amount;
            OnHealthDecreaseEvent?.Invoke();
            CheckDie();
        }

        public void RestoreHealth(int amount)
        {
            _currentHealth += amount;
            OnHealthInCreaseEvent?.Invoke();
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
    }

}