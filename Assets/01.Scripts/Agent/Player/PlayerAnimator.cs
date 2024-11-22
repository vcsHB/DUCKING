using System;
using UnityEngine;

namespace AgentManage.PlayerManage
{
    public class PlayerAnimator : AgentAnimator
    {
        private Player _player;

        public override void Initialize(Agent agent)
        {
            base.Initialize(agent);
            _player = agent as Player;
            _player.PlayerInput.MovementEvent += HandleMovement;
        }
    }

}
