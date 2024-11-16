using UnityEngine;

namespace WaveSystem
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private StageWaveSO _stage; // 근데 나중에 뭔가 랜덤 웨이브 그런거로 갈거같음.

        public int CurrentWaveIndex { get; private set; } = 0;

        public WaveSO CurrentWave { get; private set; }



        public void StartWave()
        {
            
            _enemySpawner.GenerateWaveEnemys(CurrentWave);
        }

    }
}