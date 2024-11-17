using System.Collections;
using System.Collections.Generic;
using AgentManage.Enemys;
using ObjectPooling;
using UnityEngine;

namespace WaveSystem
{

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private WaveSO _currentWave;

        [SerializeField] private Transform _targetPosition;

        public List<Enemy> EnemyList {get; private set; } = new List<Enemy>();

        private void Start()
        {
            PathFinder.FindPath(transform.position, _targetPosition.position);
        }


        public void GenerateWaveEnemys(WaveSO wave)
        {
            _currentWave = wave;

            StartCoroutine(GenerateEnemyCoroutine());
        }

        private IEnumerator GenerateEnemyCoroutine()
        {
            for (int i = 0; i < _currentWave.waveInfos.Length; i++)
            {
                WaveInfo wave = _currentWave.waveInfos[i];
                PoolingType poolingType = wave.enemyType;
                WaitForSeconds ws = new WaitForSeconds(wave.generateTerm);
                for (int j = 0; j < wave.amount; j++)
                {
                    Enemy enemy = PoolManager.Instance.Pop(poolingType, transform.position, Quaternion.identity) as Enemy;
                    enemy.GetCompo<EnemyAI>().SetMove();
                    EnemyList.Add(enemy);
                    yield return ws;
                }
            }
        }

        private void HandleEnemyDie(Enemy enemy)
        {
            EnemyList.Remove(enemy);
            enemy.OnEnemyDieEvent -= HandleEnemyDie;
        }

    }

}