using Unity.VisualScripting;
using UnityEngine;
namespace AgentManage.Enemies
{


    public abstract class EnemyState
    {
        protected Enemy _owner;
        protected EnemyStateMachine _stateMachine;
        protected bool _endTriggerCalled;
        protected int _animBoolHash;
        private Enemy enemy;

        public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolHash)
        {
            _owner = enemy;
            _stateMachine = stateMachine;
            _animBoolHash = Animator.StringToHash(animBoolHash);
        }

        public EnemyState(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public virtual void Enter()
        {

        }

        public virtual void UpdateState()
        {

        }

        public virtual void Exit()
        {

        }

    }

}