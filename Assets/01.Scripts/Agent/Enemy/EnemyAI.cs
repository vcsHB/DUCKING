using System.Collections.Generic;
using UnityEngine;

namespace AgentManage.Enemies
{
    public class EnemyAI : MonoBehaviour, IAgentComponent
    {
        private Enemy _enemy;
        private List<Vector2> _path;
        private int _currentPathIndex = 0;
        private float _moveSpeed;
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
            _moveSpeed = _enemy.Stat.moveSpeed.GetValue();
        }

        public void SetMove()
        {
            _path = PathFinder.GetPath;
            _currentPathIndex = 0;
            CanMove = true;
        }

        // ========== State Handles =====================

        //internal void HandleSetEnemy

        public void HandleMoveToPath()
        {
            if (_path.Count > 0 && _currentPathIndex < _path.Count)
            {
                MoveAlongPath();
            }
        }


        // ===========================================

        private void MoveAlongPath() // Update
        {
            Vector3 target = _path[_currentPathIndex];
            transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.1f) // 거의 도착시 다음 PATH로 이동...  
            {
                _currentPathIndex++;
            }
        }

    }

}