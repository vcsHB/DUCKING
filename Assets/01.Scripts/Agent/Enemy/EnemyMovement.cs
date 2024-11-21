using System;
using UnityEngine;

namespace AgentManage.Enemies
{
    public class EnemyMovement : MonoBehaviour, IAgentComponent
    {
        private Rigidbody2D _rigidCompo;
        private Enemy _enemy;
        private float _moveSpeed;
        public event Action<Vector2> OnMovementEvent;


        private void Awake()
        {
            _rigidCompo = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Agent agent)
        {
            _enemy = agent as Enemy;
            _moveSpeed = _enemy.Stat.moveSpeed.GetValue();

        }

        public void AfterInit()
        {
        }

        public void Dispose()
        {
        }

        public void StopImmediately()
        {
            _rigidCompo.velocity = Vector2.zero;
        }

        public void Move(Vector2 moveDir)
        {
            OnMovementEvent?.Invoke(moveDir);
            _rigidCompo.velocity = moveDir * _moveSpeed;
        }
    }
}