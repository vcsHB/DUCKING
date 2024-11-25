using InputManage;
using UI.InGame.GameSystem;
using UnityEngine;

namespace AgentManage.PlayerManage
{

    public class Player : Agent
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        public PlayerMovement MovementCompo { get; protected set; }
        public PlayerItemCollector ItemCollectCompo { get; protected set; }

        protected override void Awake()
        {
            base.Awake();
            MovementCompo = GetCompo<PlayerMovement>();
            ItemCollectCompo = GetComponent<PlayerItemCollector>();
            HealthCompo.OnDieEvent.AddListener(HandleGameOver);
        }

        private void HandleGameOver()
        {
            UIManager.Instance.ShowFailedPanel();
        }

    }
}