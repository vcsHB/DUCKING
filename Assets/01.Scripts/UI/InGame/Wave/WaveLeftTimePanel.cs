using TMPro;
using UnityEngine;

namespace UI.InGame.Wave
{

    public class WaveLeftTimePanel : UIPanel
    {

        [SerializeField] private TextMeshProUGUI _leftTimeText;

        private readonly string _nextWaveMsg = "다음 웨이브까지 ";

        public void HandleRefreshLeftTimeText(int leftSecond)
        {
            _leftTimeText.text = $"_nextWaveMsg{leftSecond.ToString()}";
        }

    }

}