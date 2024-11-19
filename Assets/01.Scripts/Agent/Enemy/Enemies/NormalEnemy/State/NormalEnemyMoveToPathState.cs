using UnityEngine;
namespace AgentManage.Enemies.State
{

    public class NormalEnemyMoveToPathState : EnemyState
    {
        private EnemyAI _movePathAI;
        private EnemyTargetDetector _targetDetector;

        public NormalEnemyMoveToPathState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolHash) : base(enemy, stateMachine, animBoolHash)
        {
            _movePathAI = enemy.GetCompo<EnemyAI>();
            _targetDetector = enemy.GetCompo<EnemyTargetDetector>();
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("MoveToPath Enter");
        }

        public override void UpdateState()
        {
            base.UpdateState();
            Debug.Log("MoveToPath Update");
            if (!_movePathAI.CanMove) return;

            _movePathAI.HandleMoveToPath();
            CheckTarget();
        }

        private void CheckTarget()
        {
            if (_targetDetector.IsTargeting)
            {
                Debug.Log("공격 스테이트로 전환");
                _stateMachine.ChangeState(_stateMachine.GetState("Attack"));
            }
        }


    }
}