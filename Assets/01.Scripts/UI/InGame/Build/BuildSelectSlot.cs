using System;
using BuildingManage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Build
{
    public class BuildSelectSlot : MonoBehaviour
    {
        private Button _button;
        [SerializeField] private Image _buildingIconImage;
        [SerializeField] private BuildingSO _buildingInfo;
        public event Action<BuildingSO> OnClickEvent;

        
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleButtonClick);
        }

        public void Initialize(BuildingSO buildingInfo)
        {
            _buildingInfo = buildingInfo;
            _buildingIconImage.sprite = buildingInfo.buildingIconSprite;
            gameObject.SetActive(true);
            
        }

        private void HandleButtonClick()
        {
            OnClickEvent?.Invoke(_buildingInfo);
        }


        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}