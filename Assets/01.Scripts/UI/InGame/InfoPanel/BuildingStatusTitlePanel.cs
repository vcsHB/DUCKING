using AgentManage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI.InGame.BuildingInfoDisplayer
{

    public class BuildingStatusTitlePanel : MonoBehaviour
    {
        [SerializeField] private Image _buildingIconImage;
        [SerializeField] private Image _buildingHealthGauge;
        [SerializeField] private TextMeshProUGUI _buildingNameText;

        public void HandleRefreshHealthGauge(int currentHealth, int max)
        {
            _buildingHealthGauge.fillAmount = (float)currentHealth / max;
        }

        public void SetBuildingInfo(string nameString, Sprite buildingIcon)
        {
            _buildingIconImage.sprite = buildingIcon;
            _buildingNameText.text = nameString;
        }
    }
}