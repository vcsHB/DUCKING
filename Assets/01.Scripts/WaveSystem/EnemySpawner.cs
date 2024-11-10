using AgentManage.Enemys;
using ObjectPooling;
using UnityEngine;

namespace WaveSystem
{

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private StageWaveSO _stage; // 근데 나중에 뭔가 랜덤 웨이브 그런거로 갈거같음.
        [SerializeField] private WaveSO _currentWave;

        [SerializeField] private Transform _targetPosition;

        private void Start()
        {
            PathFinder.FindPath(transform.position, _targetPosition.position);
        }

        [ContextMenu("DebugGenerateEnemy")]
        private void GenerateEnemeys()
        {

            for (int i = 0; i < _currentWave.waveInfos.Length; i++)
            {
                WaveInfo wave = _currentWave.waveInfos[i];
                PoolingType poolingType = wave.enemyType;
                for (int j = 0; j < wave.amount; j++)
                {
                    Enemy enemy = PoolManager.Instance.Pop(poolingType, transform.position, Quaternion.identity) as Enemy;
                    enemy.GetCompo<EnemyAI>().SetMove();
                }
            }
        }

    }

}