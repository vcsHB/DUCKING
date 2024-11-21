using AgentManage;
using AgentManage.Enemies;
using Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideEnemyBombState : EnemyState
{
    private Collider2D[] _coll;
    private Transform _enemyTrm;
    private Health _health;
    private float _explosibilityRange = 3f;

    public SuicideEnemyBombState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolHash) : base(enemy, stateMachine, animBoolHash)
    {
        _enemyTrm = enemy.transform;
        _coll = new Collider2D[9];
        _health = enemy.GetComponent<Health>();
    }

    public override void Enter()
    {
        base.Enter();

        //Æø¹ß ÀÌÆåÆ® ³ª¿À°Ô ÇÏ±â
        int cnt = Physics2D.OverlapBoxNonAlloc(_enemyTrm.position, Vector2.one * _explosibilityRange, 0, _coll);

        for (int i = 0; i < cnt; i++)
        {
            bool canHit = _coll[i].TryGetComponent(out IDamageable damageable);

            if (canHit)
            {
                damageable.ApplyDamage(3);
            }
        }

        _health.ApplyDamage(int.MaxValue);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
