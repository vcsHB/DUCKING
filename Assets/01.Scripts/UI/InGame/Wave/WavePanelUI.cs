using TMPro;
using UnityEditor;
using UnityEngine;
using WaveSystem;

namespace UI.InGame.Wave
{

    public class WavePanelUI : MonoBehaviour
    {
        [SerializeField] private WaveLeftTimePanel _leftTimePanel;
        [SerializeField] private WaveInfoPanel _waveInfoPanel;
        [SerializeField] private EnemySpawner _enemySpawner;

        public void SetWaitingWave()
        { // 웨이브 대기 상태로 전환
            _waveInfoPanel.Close();
            _leftTimePanel.Open();
        }

        public void SetStartWave()
        {
            _waveInfoPanel.Open();
            _leftTimePanel.Close();
        }

        public void HandleRefreshLeftWaitTime(float leftTime)
        {
            _leftTimePanel.HandleRefreshLeftTimeText((int)leftTime);
        }


        public void HandleRefreshWaveInfo(int waveIndex, int enemyAmount)
        {
            _waveInfoPanel.SetWaveInfo(waveIndex, enemyAmount);
        }


    }
}