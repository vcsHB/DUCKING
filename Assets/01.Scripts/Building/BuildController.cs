using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace BuildingManage
{
    public class BuildController : MonoBehaviour
    {
        private BuildingSetSO _buildingSet;
        private Transform _buildingParent;

        [SerializeField] private BuildingEnum _buildTarget;
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
                int size = _buildingSet.FindBuilding(_buildTarget).tileSize;
                BuildingSize fabricSize = new BuildingSize(MapManager.Instance.GetTilePos(position), size);

                bool canBuild = MapManager.Instance.CheckBuildingOverlap(fabricSize);
                _buildingPreview.UpdateBuildidng(position, size);
            }

            if (_tryBuild && Input.GetMouseButtonDown(0))
            {
                Vector2Int position
                    = MapManager.Instance.GetTilePos(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                Build(_buildTarget, position, true);
            }
        }

        public void SetBuildTarget(BuildingEnum buildingType)
        {
            _buildTarget = buildingType;
            _tryBuild = true;
            SetPreview(true);
        }

        public bool Build(BuildingEnum building, Vector2Int position, bool save)
        {
            BuildingSO info = _buildingSet.FindBuilding(building);
            BuildingSize size = new BuildingSize(position, info.tileSize);

            bool isOverlap = MapManager.Instance.CheckBuildingOverlap(size);
            if (isOverlap) return false;

            info.building.Build(position, _curDirection, save);

            return true;
        }

        private void SetPreview(bool isEnable)
        {
            if (isEnable)
            {
                int tileSize = _buildingSet.FindBuilding(_buildTarget).tileSize;
                _buildingPreview.SetBuilding(tileSize);
            }
            else
            {
                _buildingPreview.Disable();
            }
        }

        public void Init(BuildingSetSO buildingSet)
        {
            _buildingSet = buildingSet;
        }
    }
}

