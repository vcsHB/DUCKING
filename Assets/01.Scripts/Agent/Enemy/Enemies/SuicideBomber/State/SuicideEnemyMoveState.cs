using AgentManage.Enemies;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuicideEnemyMoveState : EnemyState
{
    private EnemyAI _movePathAI;
    private Transform _enemyTrm;
    private EnemyTargetDetector _targetDetector;
    private SuicideBomberEnemy bomber;

    public SuicideEnemyMoveState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolHash) : base(enemy, stateMachine, animBoolHash)
    {
        bomber = enemy as SuicideBomberEnemy;
        _movePathAI = enemy.GetCompo<EnemyAI>();
        _enemyTrm = enemy.transform;
        _targetDetector = enemy.GetCompo<EnemyTargetDetector>();
    }

    public override void Enter()
    {
        base.Enter();

        if (bomber.target != null)
        {
            PathFinder.FindPath(_enemyTrm.position, bomber.target.position);
            _movePathAI.SetMove();
        }
    }

    public override void UpdateState()
    {
        if (!_movePathAI.CanMove) return;

        _movePathAI.HandleMoveToPath();
        CheckTarget();
    }

    private void CheckTarget()
    {
        Debug.Log(_targetDetector);
        Debug.Log(_targetDetector.IsTargeting);

        if (_targetDetector.IsTargeting)
        {
            Debug.Log("밍글링1");
            bomber.SetTartget(_targetDetector.CurrentTarget);
            Debug.Log("밍글링2");
            _stateMachine.ChangeState(_stateMachine.GetState("RunToTarget"));
            Debug.Log("밍글링3");
        }
    }
}