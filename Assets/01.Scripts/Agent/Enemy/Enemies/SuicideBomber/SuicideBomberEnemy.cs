using ObjectPooling;
using UnityEngine;

namespace AgentManage.Enemies
{
    public class SuicideBomberEnemy : StateEnemy
    {
        public Vector3 TargetPosition { get; private set; }
        public BomberTimer timer;

        protected override void InitStates()
        {
            StateMachine.AddState("Move", new SuicideEnemyMoveState(this, StateMachine, "Move"));
            StateMachine.AddState("RunToTarget", new SuicideEnemyRunToTargetState(this, StateMachine, "RunToTarget"));
            StateMachine.AddState("Bomb", new SuicideEnemyBombState(this, StateMachine, "Bomb"));

            StateMachine.Initialize("Move", this);
            timer.EndTimer();
        }

        public override void Initialize()
        {
            StateMachine.ChangeState(StateMachine.GetState("Move"));
        }
    }
}

