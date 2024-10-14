using StatSystem;
using UnityEngine;

namespace AgentManage
{
    
    public abstract class Agent : MonoBehaviour
    {
        public Health HealthCompo { get; private set; }
        [field:SerializeField] public StatusSO Stat { get; protected set; }


        protected virtual void Awake()
        {
            Stat = Instantiate(Stat);
            HealthCompo = GetComponent<Health>();
        }
    }

}