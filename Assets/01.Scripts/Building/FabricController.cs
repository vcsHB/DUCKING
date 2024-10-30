using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingManage
{
    public class FabricController : MonoBehaviour
    {
        private FabricSetSO _buildingSet;
        private Transform _buildingParent;

        [SerializeField] private FabricEnum _testBuilding;
        [SerializeField] private DirectionEnum _curDirection;

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector2Int position
                    = MapManager.Instance.GetTilePos(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                Build(_testBuilding, position);
            }
        }

        public void Build(FabricEnum building, Vector2Int position)
        {
            FabricSO info = _buildingSet.FindBuilding(building);
            info.building.Build(position, _curDirection);
        }

        public void Init(FabricSetSO buildingSet)
        {
            _buildingSet = buildingSet;
        }
    }
}

