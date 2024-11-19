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
        private bool _canMove = false;

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
            _canMove = true;
        }

        private void Update()
        {
            if(!_canMove) return;

            if (_path.Count > 0 && _currentPathIndex < _path.Count)
            {
                MoveAlongPath();
            }
        }

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