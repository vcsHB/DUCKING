﻿using System;
using System.Collections.Generic;
using AgentManage.PlayerManage;
using ItemSystem;
using ResourceSystem;
using UnityEngine;

namespace UI.InGame.Build
{
    public class BuildMaterialPanel : MonoBehaviour
    {
        [SerializeField] private ItemInfoGroupSO _itemInfoGroup;
        [SerializeField] private BuildMaterialSlot _slotPrefab;
        private List<BuildMaterialSlot> _slotList = new List<BuildMaterialSlot>();
        [SerializeField] private Transform _contentTrm;
        [SerializeField] private PlayerItemCollector _playerItemCollector;

        private void Awake()
        {
            for (int i = 0; i < 12; i++)
            {
                BuildMaterialSlot slot = Instantiate(_slotPrefab, _contentTrm);
                _slotList.Add(slot);
                slot.Disable();
            }
        }

        private void Update()
        {
            _slotList.ForEach(slot =>
            {
                if (slot.ItemInfo != null)
                {
                    slot.UpdateHaveAmount(
                        _playerItemCollector.GetItemAmount(slot.ItemInfo.id));
                }
            });
        }


        public void SelectStructure()
        {// 건물 정보를 받아 재료에 대한 정보를 HandleSetMaterialSlots로 넘긴다


        }

        /**
         * <summary>
         * 건물 버튼을 눌러 재료 창을 구성해주는 상태
         * </summary>
         */
        public void HandleSetMaterialSlots(Resource[] infos)
        {
            // 재료요구량이 앵간하면 12종을 넘지 않을테지만 설마 그렇게 넣는 경우에는 UX 사이즈에 문제 발생가능하기 때문에 걍 리턴
            if (infos.Length > _slotList.Count) return;
            DisableAllSlot();
            for (int i = 0; i < infos.Length; i++)
            {
                ItemInfoSO info = _itemInfoGroup.GetItemData((int)infos[i].type); // 자원 enum과 Item id를 동기화 시켜야함.
                _slotList[i].SetMaterial(info, infos[i].amount, _playerItemCollector.GetItemAmount(info.id));
            }

        }

        private void DisableAllSlot()
        {
            foreach (BuildMaterialSlot slot in _slotList)
            {
                slot.Disable();
            }
        }
    }
}