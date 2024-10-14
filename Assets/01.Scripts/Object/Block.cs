using AgentManage;
using StatSystem;
using UnityEngine;

namespace BlockManage
{
    
    public abstract class Block : MonoBehaviour
    {
        public Health HealthCompo { get; protected set; }
        [field:SerializeField] public StatusSO Stat { get; protected set; }

        protected virtual void Awake()
        {
            Stat = Instantiate(Stat);
            HealthCompo = GetComponent<Health>();
        }
    }

}