using AgentManage;
using StatSystem;
using UnityEngine;

namespace BuildingManage
{
    
    public abstract class Block : MonoBehaviour, IDamageable
    {
        public Health HealthCompo { get; protected set; }
        [field:SerializeField] public StatusSO Stat { get; protected set; }

        public void ApplyDamage(int amount)
        {

        }

        protected virtual void Awake()
        {
            Stat = Instantiate(Stat);
            HealthCompo = GetComponent<Health>();
        }
    }

}