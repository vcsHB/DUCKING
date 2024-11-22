using AgentManage;
using AgentManage.Enemies;
using UnityEngine;

public class SuicideEnemyBombState : EnemyState
{
    private Collider2D[] _coll;
    private Transform _enemyTrm;
    private Health _health;
    private float _explosibilityRange = 3f;
    private SuicideBomberEnemy _bombEnemy;

    public SuicideEnemyBombState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolHash) : base(enemy, stateMachine, animBoolHash)
    {
        _enemyTrm = enemy.transform;
        _bombEnemy = enemy as SuicideBomberEnemy;
        _coll = new Collider2D[9];
        _health = enemy.GetComponent<Health>();
    }

    public override void Enter()
    {
        base.Enter();

        _bombEnemy.Explode();
        _health.ApplyDamage(int.MaxValue);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
