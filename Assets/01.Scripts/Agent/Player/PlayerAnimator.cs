using System;
using UnityEngine;

namespace AgentManage.PlayerManage
{
    public class PlayerAnimator : MonoBehaviour, IAgentComponent
    {
        private Player _player;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private int _walkAnimationHash;
        private int _idleAnimationHash;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

        }

        private void HandleMovement(Vector2 direction)
        {
            print("밍미임");
            _animator.SetFloat(_walkAnimationHash, direction.magnitude);
            HandleApplyMovementDirectionRotation(direction.x);
        }

        private void HandleApplyMovementDirectionRotation(float x)
        {
            if(x == 0) return;
            _spriteRenderer.flipX = x < 0;
        }

        public void Initialize(Agent agent)
        {
            _player = agent as Player;

            _player.PlayerInput.MovementEvent += HandleMovement;
            _walkAnimationHash = Animator.StringToHash("Speed");
        }

        public void AfterInit()
        {
        }

        public void Dispose()
        {
        }
    }

}
