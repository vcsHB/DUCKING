using UnityEngine;
namespace AgentManage.Enemies.State
{

    public class NormalEnemyAttackState : EnemyState
    {
        public NormalEnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolHash) : base(enemy, stateMachine, animBoolHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Attack Enter");
        }


    }
}