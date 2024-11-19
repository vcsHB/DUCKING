using UnityEngine;

namespace AgentManage.Enemies
{

    public abstract class StateEnemy : Enemy
    {
        public EnemyStateMachine StateMachine { get; protected set; }

        protected override void Awake()
        {
            base.Awake();
            StateMachine = new EnemyStateMachine();
            InitStates();
        }

        protected virtual void Update()
        {
            StateMachine.UpdateState();
        }

        protected abstract void InitStates();

    }

}