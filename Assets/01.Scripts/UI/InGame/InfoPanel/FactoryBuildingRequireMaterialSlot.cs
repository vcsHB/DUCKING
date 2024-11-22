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

        public void HandleRefreshSlot(Resource resource)
        {

        }
    }
}