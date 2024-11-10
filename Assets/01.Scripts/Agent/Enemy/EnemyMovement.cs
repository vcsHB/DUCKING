using UnityEngine;

namespace AgentManage.Enemys
{
    public class EnemyMovement : MonoBehaviour, IAgentComponent
    {
        private Rigidbody2D _rigidCompo;
        private Enemy _enemy;


        private void Awake()
        {
            _rigidCompo = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Agent agent)
        {
            _enemy = agent as Enemy;

        }

        public void AfterInit()
        {
        }

        public void Dispose()
        {
        }
    }
}