using System;
using ResourceSystem;
using TMPro;
using UI.InGame.BuildingInfoDisplayer.FactoryDetail;
using UnityEngine;
using UnityEngine.UI;
namespace UI.InGame.BuildingInfoDisplayer
{

    public class FactoryBuildingDetailInfoPanel : MonoBehaviour
    {

        [SerializeField] private Image _progressBarImage;
        [SerializeField] private TextMeshProUGUI _progressAndLeftTimeText;
        [SerializeField] private FactoryBuildingRequireMaterialSlot[] _requireMaterialSlots;
        private Factory _owner;

        public void Initialize(Factory factory)
        {
            if (_owner != null)
            {
                _owner.OnProgressEvent -= HandleProgress;
                _owner.OnStorageChanged -= HandleStorageChanged;
            }
            _owner = factory;
            _owner.OnProgressEvent += HandleProgress;
            _owner.OnStorageChanged += HandleStorageChanged;
            // for(int i = 0; i < _owner.RequireResources.Keys.Count; i++)
            // {
            //     HandleStorageChanged()

            // }

        }

        private void HandleStorageChanged(ResourceType type, int currentAmount, int needAmount)
        {

        }


        public void HandleProgress(float currentTime, float processTime)
        {
            float ratio = Mathf.Clamp01(currentTime / processTime);
            _progressBarImage.fillAmount = ratio;
            _progressAndLeftTimeText.text = $"진행도 ({ratio * 100}%) 남은 시간 ({processTime - currentTime}s)";

        }



    }
}