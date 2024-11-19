using UnityEngine;
namespace AgentManage.Enemies.State
{

    public class NormalEnemyIdleState : EnemyState
    {
        public NormalEnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolHash) : base(enemy, stateMachine, animBoolHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Idle Enter");
        }


    }
}