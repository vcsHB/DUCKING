using TMPro;
using UnityEngine;

namespace UI.InGame.Wave
{

    public class WaveInfoPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI _currentWaveText;
        [SerializeField] private TextMeshProUGUI _leftEnemyAmountText;


        

        public void SetWaveInfo (int currentWave, int leftEnemyAmount)
        {
            _currentWaveText.text = $"{currentWave.ToString()} 웨이브";
            _leftEnemyAmountText.text = $"{leftEnemyAmount} 개체 남음";

        }

    }

}