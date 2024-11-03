using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BuildingManage
{
    public class FabricController : MonoBehaviour
    {
        private FabricSetSO _buildingSet;
        private Transform _buildingParent;

        [SerializeField] private FabricEnum _testBuilding;
        [SerializeField] private DirectionEnum _curDirection;
        [SerializeField] private BuildingPreview _buildingPreview;
        private bool _tryBuild;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                _tryBuild = !_tryBuild;
                SetPreview(_tryBuild);
            }

            if (_tryBuild)
            {
                Vector2 position = Input.mousePosition;
                int size = _buildingSet.FindBuilding(_testBuilding).tileSize;
                FabricSize fabricSize = new FabricSize(MapManager.Instance.GetTilePos(position), size);

                bool canBuild = MapManager.Instance.CheckBuildingOverlap(fabricSize);
                _buildingPreview.UpdateBuildidng(position, size);
            }

            if (_tryBuild && Input.GetMouseButtonDown(0))
            {
                Vector2Int position
                    = MapManager.Instance.GetTilePos(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                Build(_testBuilding, position, true);
            }
        }

        public bool Build(FabricEnum building, Vector2Int position, bool save)
        {
            FabricSO info = _buildingSet.FindBuilding(building);
            FabricSize size = new FabricSize(position, info.tileSize);

            bool isOverlap = MapManager.Instance.CheckBuildingOverlap(size);
            if (isOverlap) return false;

            info.building.Build(position, _curDirection, save);

            return true;
        }

        private void SetPreview(bool isEnable)
        {
            if (isEnable)
            {
                int tileSize = _buildingSet.FindBuilding(_testBuilding).tileSize;
                _buildingPreview.SetBuilding(tileSize);
            }
            else
            {
                _buildingPreview.Disable();
            }
        }

        public void Init(FabricSetSO buildingSet)
        {
            _buildingSet = buildingSet;
        }
    }
}

