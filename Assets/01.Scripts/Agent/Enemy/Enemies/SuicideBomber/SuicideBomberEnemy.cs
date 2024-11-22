using Combat;
using ObjectPooling;
using UnityEngine;

namespace AgentManage.Enemies
{
    public class SuicideBomberEnemy : StateEnemy, IExplodable
    {
        public Vector3 TargetPosition { get; private set; }
        public BomberTimer timer;
        public DamageCaster DamageCaster { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            DamageCaster = GetComponentInChildren<DamageCaster>();
        }

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

        public void Explode()
        {
            VFXPlayer vfx = PoolManager.Instance.Pop(PoolingType.SmallExplosionVFX, transform.position, Quaternion.identity) as VFXPlayer;
            vfx.PlayVFX();
            DamageCaster.CastDamage();
        }


    }
}

