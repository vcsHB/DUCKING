using AgentManage.Enemies;
using UnityEngine;

public class SuicideEnemyRunToTargetState : EnemyState
{
    private EnemyAI _movePathAI;
    private Transform _enemyTrm;
    private EnemyTargetDetector _targetDetector;
    private SuicideBomberEnemy _bomber;
    private BomberTimer _timer;

    private float _enterTimer;
    private readonly int _bombCount = 3;

    public SuicideEnemyRunToTargetState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolHash) : base(enemy, stateMachine, animBoolHash)
    {
        _enemyTrm = enemy.transform;
        _bomber = enemy as SuicideBomberEnemy;
        _movePathAI = enemy.GetCompo<EnemyAI>();
        _targetDetector = enemy.GetCompo<EnemyTargetDetector>();

        _timer = _bomber.timer;
    }

    public override void Enter()
    {
        Debug.Log("123");

        _timer.StartTimer();
        _enterTimer = Time.time;
        if (_bomber.target != null)
        {
            Debug.Log("นึ1");
            PathFinder.FindPath(_enemyTrm.position, _bomber.target.position);
            _movePathAI.SetMove();
            Debug.Log("นึ2");
        }
        Debug.Log("นึ");
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_movePathAI.CanMove)
            _movePathAI.HandleMoveToPath();


        _timer.SetTimer(_bombCount - Mathf.RoundToInt(Time.time - _enterTimer));

        if (_enterTimer + _bombCount < Time.time)
        {
            _stateMachine.ChangeState(_stateMachine.GetState("Bomb"));
        }
    }
}
