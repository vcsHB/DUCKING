using System;
using System.Collections.Generic;
using BuildingManage;
using UnityEngine;

namespace UI.InGame.Build
{
    public class BuildSelectPanel : MonoBehaviour
    {
        [SerializeField] private BuildDetailPanel _detailPanel;
        [SerializeField] private BuildSelectSlot _slotPrefab;
        [SerializeField] private Transform _contentTrm;
        private List<BuildSelectSlot> _slotList = new List<BuildSelectSlot>();

        private void Awake()
        {
            
        }

        public void SetDisplayBuildingSlots(BuildingSO[] buildingList)
        {
            DisableAllSlots();

            int slotShortage = buildingList.Length - _slotList.Count;
            if (slotShortage > 0)
            {
                for (int i = 0; i < slotShortage; i++)
                {
                    BuildSelectSlot slot = Instantiate(_slotPrefab, _contentTrm);
                    slot.OnClickEvent += HandleBuildingSelect;
                    slot.Disable();
                    _slotList.Add(slot);
                    
                }
            }

            for (int i = 0; i < buildingList.Length; i++)
            {
                _slotList[i].Initialize(buildingList[i]);
            }
            
        }

        private void DisableAllSlots()
        {
            foreach (BuildSelectSlot slot in _slotList)
            {
                slot.Disable();
            }            
        }

        public void HandleBuildingSelect(BuildingSO buildingInfo)
        {
            _detailPanel.Open();
            _detailPanel.HandleSettingBuildDetail(buildingInfo);
        }
        
        
    }
}