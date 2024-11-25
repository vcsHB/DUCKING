using System.Resources;
using ResourceSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI.InGame.BuildingInfoDisplayer.FactoryDetail
{


    public class FactoryBuildingRequireMaterialSlot : MonoBehaviour
    {
        [SerializeField] private Image _materialIconIamge;
        [SerializeField] private TextMeshProUGUI _materialAmountText;

        public void SetEnable(bool value)
        {
            gameObject.SetActive(value);
        }

        public void HandleRefreshSlot(Sprite sprite, int amount, int require)
        {
            SetEnable(true);
            _materialIconIamge.sprite = sprite;
            _materialAmountText.color = amount < require ? Color.red : Color.white;
            _materialAmountText.text = $"{amount.ToString()}/{require.ToString()}";
        }
    }
}