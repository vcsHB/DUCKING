using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Wave
{

    public class WaveLeftTimePanel : UIPanel
    {

        [SerializeField] private TextMeshProUGUI _leftTimeText;
        [SerializeField] private Image _leftTimeGaugeImage;

        private readonly string _nextWaveMsg = "다음 웨이브까지 ";

        public void HandleRefreshLeftTimeText(int leftSecond, float ratio)
        {
            _leftTimeText.text = $"{_nextWaveMsg} {leftSecond.ToString()}s";
            _leftTimeGaugeImage.fillAmount = ratio;
        }

    }

}