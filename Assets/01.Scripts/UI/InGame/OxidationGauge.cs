using AgentManage;
using AgentManage.PlayerManage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame
{
    public class OxidationGauge : MonoBehaviour
    {
        [SerializeField] private Player _player;
        private CorrosiveHealth _health;

        [SerializeField] private Image _fillImage;
        [Header("Color Setting")]
        [Tooltip("부식 전 색상")]
        [SerializeField] private Color _startColor;
        [Tooltip("부식 끝 색상")]
        [SerializeField] private Color _endColor;


        private void Start()
        {
            _health = _player.CorrosiveHealth;
            _health.OnCorrosionChangedEvent += HandleCorrosionChange;
            HandleCorrosionChange(1, 1);
        }

        private void HandleCorrosionChange(int currentValue, int maxValue)
        {
            float ratio = (float)currentValue / maxValue;
            _fillImage.fillAmount = ratio;
            _fillImage.color = Color.Lerp(_endColor, _startColor, ratio);

        }
    }

}