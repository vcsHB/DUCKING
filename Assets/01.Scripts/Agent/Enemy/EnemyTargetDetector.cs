using UnityEngine;
namespace AgentManage.Enemies
{

    public class EnemyTargetDetector : MonoBehaviour, IAgentComponent
    {
        [SerializeField] private float _detectRange = 3f;
        [SerializeField] private LayerMask _targetMask;
        private Enemy _enemy;
        private Collider2D[] _hits;
        public Transform CurrentTarget;
        public Vector2 TargetDirection { get; private set; }
        public bool IsTargeting { get; private set; }

        private void Awake()
        {
            _hits = new Collider2D[1];
        }

        public void Initialize(Agent agent)
        {
            _enemy = agent as Enemy;
        }

        private void Update()
        {
            int amount = Physics2D.OverlapCircleNonAlloc(transform.position, _detectRange, _hits, _targetMask);
            
            IsTargeting = amount > 0;
            if (IsTargeting)
            {
                CurrentTarget = _hits[0].transform;
                TargetDirection = CurrentTarget.position - transform.position;
            }
            else
            {
                CurrentTarget = null;
                TargetDirection = Vector2.zero;
            }
        }


        public void AfterInit()
        {
        }

        public void Dispose()
        {
        }


    }
}