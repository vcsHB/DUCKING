using UnityEngine;
using ObjectPooling;
using Combat;

namespace AgentManage.Enemies.State
{

    public class NormalEnemyAttackState : EnemyState
    {
        private EnemyTargetDetector _targetDetector;
        private EnemyMovement _movement;
        private Transform _enemyTrm;
        private bool _isAttacking = false;

        private PoolingType _projectile;
        private float _attackDelay;
        private float _prevAttack;
        private NormalEnemy _enemy;

        public NormalEnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolHash) : base(enemy, stateMachine, animBoolHash)
        {
            _enemy = enemy as NormalEnemy;
            _movement = enemy.GetComponent<EnemyMovement>();
            _targetDetector = enemy.GetCompo<EnemyTargetDetector>();
            _attackDelay = (enemy as NormalEnemy).attackDelay;
            _projectile = (enemy as NormalEnemy).projectile;
            _enemyTrm = enemy.transform;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Attack Enter");
            _movement.StopImmediately();
            _prevAttack = Time.time;
        }

        public override void UpdateState()
        {
            base.UpdateState();
            CheckTarget();
        }

        public override void Exit()
        {
            base.Exit();
            _isAttacking = false;
            //enemy
        }

        private void CheckTarget()
        {
            if (!_isAttacking && !_targetDetector.IsTargeting)
            {
                _stateMachine.ChangeState(_stateMachine.GetState("MoveToPath"));
                return;
            }

            if (_prevAttack + _attackDelay < Time.time)
            {
                _prevAttack = Time.time;
                Projectile projectile =
                    PoolManager.Instance.Pop(_projectile, _enemyTrm.position, Quaternion.identity)
                    as Projectile;
                _enemy.HandleAttackEvent();
                projectile.Fire(_targetDetector.TargetDirection);
            }
        }
    }
}