using AgentManage.Enemies.State;
using ObjectPooling;
using UnityEngine.Events;

namespace AgentManage.Enemies
{
    public class NormalEnemy : StateEnemy
    {
        public UnityEvent OnEnemyAttackEvent;
        public PoolingType projectile;
        public float attackDelay;

        protected override void InitStates()
        {
            StateMachine.AddState("Idle", new NormalEnemyIdleState(this, StateMachine, "Idle"));
            StateMachine.AddState("MoveToPath", new NormalEnemyMoveToPathState(this, StateMachine, "MoveToPath"));
            StateMachine.AddState("Chasing", new NormalEnemyChasingState(this, StateMachine, "Chasing"));
            StateMachine.AddState("Attack", new NormalEnemyAttackState(this, StateMachine, "Attack"));

            StateMachine.Initialize("MoveToPath", this);
        }

        public override void Initialize()
        {
            StateMachine.ChangeState(StateMachine.GetState("MoveToPath"));
        }

        internal void HandleAttackEvent()
        {
            OnEnemyAttackEvent?.Invoke();
        }


    }
}