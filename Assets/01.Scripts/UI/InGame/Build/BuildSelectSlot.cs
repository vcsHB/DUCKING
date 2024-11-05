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
        public event Action<FabricSO> OnClickEvent;

        
        
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
            OnClickEvent?.Invoke(_buildingInfo);
        }


        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}