using UnityEngine;
namespace AgentManage.Enemies.State
{

    public class NormalEnemyChasingState : EnemyState
    {
        public NormalEnemyChasingState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolHash) : base(enemy, stateMachine, animBoolHash)
        {
        }

        public override void Enter()
        {
            base.Enter();

        }


    }
}