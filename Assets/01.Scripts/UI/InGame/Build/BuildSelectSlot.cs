using System;
using BuildingManage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Build
{
    public class BuildSelectSlot : MonoBehaviour
    {
        private Button _button;
        [SerializeField] private FabricSO _buildingInfo;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleButtonClick);
        }

        public void Initialize(FabricSO buildingInfo)
        {
            _buildingInfo = buildingInfo;
            gameObject.SetActive(true);
            
        }

        private void HandleButtonClick()
        {
            
        }


        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}