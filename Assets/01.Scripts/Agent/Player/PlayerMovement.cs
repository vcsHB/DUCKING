using System;
using AgentManage;
using StatSystem;
using UnityEngine;

namespace AgentManage.PlayerManage
{

    public class PlayerMovement : MonoBehaviour, IAgentComponent
    {
        private Rigidbody2D _rigid;
        private Player _player;
        private Stat _playerMoveSpeedStatus;

        [SerializeField] private Vector2 _movement;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();

        }

        public void Initialize(Agent agent)
        {
            _player = agent as Player;
            _playerMoveSpeedStatus = _player.Stat.moveSpeed;
            _player.PlayerInput.MovementEvent += HandlePlayerMove;
        }

        private void HandlePlayerMove(Vector2 movement)
        {
            _movement = movement;
            print(movement);
        }

        private void FixedUpdate()
        {
            if (_movement.magnitude < 0.1f)
            {
                _movement = Vector2.zero;
            }
            _rigid.velocity = _movement.normalized * _playerMoveSpeedStatus.GetValue();
        }


        public void AfterInit() { }

        public void Dispose()
        {
            _player.PlayerInput.MovementEvent -= HandlePlayerMove;
        }
    }

}