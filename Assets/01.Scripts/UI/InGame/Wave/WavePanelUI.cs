using UnityEngine;

namespace UI.InGame.Wave
{

    public class WavePanelUI : MovePanel
    {
        [SerializeField] private WaveLeftTimePanel _leftTimePanel;
        [SerializeField] private WaveInfoPanel _waveInfoPanel;

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

        public void HandleRefreshLeftWaitTime(float currentTime, float maxTime)
        {
            float leftTime = maxTime - currentTime; 
            _leftTimePanel.HandleRefreshLeftTimeText((int)leftTime, leftTime / maxTime);
        }


        public void HandleRefreshWaveInfo(int waveIndex, int enemyAmount)
        {
            _waveInfoPanel.SetWaveInfo(waveIndex, enemyAmount);
        }

        public void HandleStageClear()
        {
            // 스테이지 클리어 했을때 따로 UI띄울 것
        }

    }
}