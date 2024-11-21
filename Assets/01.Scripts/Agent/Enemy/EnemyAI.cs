using System.Collections.Generic;
using UnityEngine;

namespace AgentManage.Enemies
{
    public class EnemyAI : MonoBehaviour, IAgentComponent
    {
        private Enemy _enemy;
        private EnemyMovement _movement;
        private List<Vector2> _path;
        private int _currentPathIndex = 0;
        public bool CanMove {get; private set;} = true;

        public void AfterInit()
        {
        }

        public void Dispose()
        {
        }

        public void Initialize(Agent agent)
        {
            _enemy = agent as Enemy;
            _movement = _enemy.GetComponent<EnemyMovement>();
        }

        public void SetMove()
        {
            _path = PathFinder.GetPath;
            _currentPathIndex = 0;
            CanMove = true;
        }

        // ========== State Handles =====================

        public void HandleMoveToPath()
        {
            if (_path.Count > 0 && _currentPathIndex < _path.Count)
            {
                MoveAlongPath();
            }
        }

        private void MoveAlongPath() // Update
        {
            Vector3 target = _path[_currentPathIndex];

            Vector2 dir = (target - transform.position).normalized;
            _movement.Move(dir);
            
            if (Vector3.Distance(transform.position, target) < 0.1f) // 거의 도착시 다음 PATH로 이동...  
            {
                _movement.StopImmediately();
                _currentPathIndex++;
            }
        }

    }

}