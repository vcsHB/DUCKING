using UI.InGame.Wave;
using UnityEngine;

namespace WaveSystem
{
    public class WaveManager : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private StageWaveSO _stage; // 근데 나중에 뭔가 랜덤 웨이브 그런거로 갈거같음.

        [Header("UI Setting")]
        [SerializeField] private WavePanelUI _wavePanel;

        public int CurrentWaveIndex { get; private set; } = 0;
        public WaveSO CurrentWave { get; private set; }

        public bool IsWaveStarted = false;
        private float _currentWaitingTime = 0;

        private void Start()
        {
            CurrentWave = _stage.waves[CurrentWaveIndex];
        }

        public void StartWave()
        {

            _enemySpawner.GenerateWaveEnemys(CurrentWave);
        }

        private void Update()
        {

            if (!IsWaveStarted) // 웨이브 진행중이 아니면. 시간 현황 띄우기 
            {
                _currentWaitingTime += Time.deltaTime;
                _wavePanel.HandleRefreshLeftWaitTime(_currentWaitingTime, CurrentWave.waveStartDelayTime);
                if (_currentWaitingTime >= CurrentWave.waveStartDelayTime)
                {
                    _currentWaitingTime = 0;
                    _enemySpawner.GenerateWaveEnemys(CurrentWave);
                    _wavePanel.SetStartWave();
                    IsWaveStarted = true;
                }
            }
            else
            {
                _wavePanel.HandleRefreshWaveInfo(CurrentWaveIndex, _enemySpawner.EnemyList.Count);
                if (_enemySpawner.EnemyList.Count > 0)
                    return;

                ContinueNextWave();
            }

        }

        private void ContinueNextWave()
        {
            CurrentWaveIndex++;
            if(CurrentWaveIndex > _stage.waves.Length)
            {
                // 웨이브 전부 클리어

                return;
            }
            CurrentWave = _stage.waves[CurrentWaveIndex];
            IsWaveStarted = false;
            _wavePanel.SetWaitingWave();
        }



    }
}