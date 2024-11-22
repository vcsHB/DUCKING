using UnityEngine;

namespace AgentManage
{
    public abstract class AgentAnimator : MonoBehaviour, IAgentComponent
    {
        private Agent _owner;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private int _walkAnimationHash;
        private int _idleAnimationHash;

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();

        }

        protected virtual void HandleMovement(Vector2 direction)
        {
            _animator.SetFloat(_walkAnimationHash, direction.magnitude);
            HandleApplyMovementDirectionRotation(direction.x);
        }

        protected void HandleApplyMovementDirectionRotation(float x)
        {
            if (x == 0) return;
            _spriteRenderer.flipX = x < 0;
        }

        public virtual void Initialize(Agent agent)
        {
            _owner = agent;

            _walkAnimationHash = Animator.StringToHash("Speed");
        }

        public virtual void AfterInit()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}