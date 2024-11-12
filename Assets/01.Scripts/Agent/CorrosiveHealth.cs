using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace AgentManage
{
    public class CorrosiveHealth : Health, ICorrosive
    {
        public UnityEvent OnCorrosionEvent;
        public event Action<int, int> OnCorrosionChangedEvent; 
        
        [SerializeField] private int _corrosionResistance;
        public int MaxCorrosionResistance { get; private set; }


        internal void SetMaxCorrosionResistance(int max)
        {
            MaxCorrosionResistance = max;
            _corrosionResistance = max;
        }
        
        public void Corrode(int power)
        {
            _corrosionResistance -= power;
            OnCorrosionEvent?.Invoke();
            OnCorrosionChangedEvent?.Invoke(_corrosionResistance, MaxCorrosionResistance);
            //print("부식 되는중 "+ _corrosionResistance +" /"+MaxCorrosionResistance);
            if (_corrosionResistance <= 0)
            {
                print("부식 딜 들어온다");
                int damage = power - _corrosionResistance;
                damage = (int)(damage + damage * ((float)power / 10));
                ApplyDamage(damage);
            }
            
        }
    }
}