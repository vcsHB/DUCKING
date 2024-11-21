using System;
using System.Collections.Generic;
using AgentManage.PlayerManage;
using ItemSystem;
using ResourceSystem;
using UnityEngine;

namespace BuildingManage
{
    public class BuildController : MonoBehaviour
    {
        public event Action OnBuildingChange;

        private BuildingSetSO _buildingSet;
        private Transform _buildingParent;

        [SerializeField] private BuildingEnum _buildTarget;
        private DirectionEnum _curDirection;
        [SerializeField] private BuildingPreview _buildingPreview;
        [SerializeField] private PlayerItemCollector _playerItemCollector;
        [SerializeField] private List<BuildingEnum> _autoRotateBuildings;

        private Vector2Int _prevPosition = new Vector2Int(int.MinValue, int.MinValue);
        private bool _isBuilding = false;
        private bool _tryBuild;

        private void Update()
        {
            Vector2Int tilePosition
                = MapManager.Instance.GetTilePos(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            //이건 나중에 바꿔줘(디버깅용?)
            if (Input.GetMouseButton(1)) TryDestroyBuilding(tilePosition);

            //이거도 바꿔야 함
            if (Input.GetKeyDown(KeyCode.P))
            {
                _tryBuild = !_tryBuild;
                SetPreview(_tryBuild);
            }

            if (!_tryBuild || _buildTarget == BuildingEnum.None) return;

            BuildingSO buildingSO = _buildingSet.FindBuilding(_buildTarget);
            SetBulidingPreview(buildingSO);
            RotateBuilding(buildingSO);

            if (CheckResource(buildingSO) == false) return;

            //new input으로 바꿔 나중에
            if (Input.GetMouseButtonDown(0)) _isBuilding = true;
            if (Input.GetMouseButtonUp(0))
            {
                _prevPosition = new Vector2Int(int.MinValue, int.MinValue);
                _isBuilding = false;
            }

            //회전을 하다!


            if (_isBuilding) TryBuild(_buildTarget, tilePosition, true);
        }

        public void SetBuildTarget(BuildingEnum buildingType)
        {
            _buildTarget = buildingType;
            _tryBuild = true;
            SetPreview(true);
        }

        public bool TryBuild(BuildingEnum building, Vector2Int position, bool save)
        {
            BuildingSO info = _buildingSet.FindBuilding(building);
            BuildingSize size = new BuildingSize(position, info.tileSize);

            bool isOverlap = MapManager.Instance.CheckBuildingOverlap(size);

            if (isOverlap) return false;

            DirectionEnum direction = info.canRotate ? _curDirection : DirectionEnum.Down;
            info.building.Build(position, direction, save);
            OnBuildingChange?.Invoke();

            if (_autoRotateBuildings.Contains(_buildTarget))
                AutoRotate(position);

            foreach (Resource resource in info.needResource)
            {
                _playerItemCollector.RemoveItem(new ItemData(resource));
            }

            return true;
        }

        public void TryDestroyBuilding(Vector2Int position)
        {
            _tryBuild = false;
            bool buildingExist =
                        MapManager.Instance.TryGetBuilding(position, out Building building);

            if (buildingExist)
            {
                OnBuildingChange?.Invoke();
                building.Destroy();
            }
        }

        public void Init(BuildingSetSO buildingSet) => _buildingSet = buildingSet;


        private void AutoRotate(Vector2Int tilePosition)
        {
            Vector2Int dir = tilePosition - _prevPosition;

            if (Mathf.Abs(dir.x + dir.y) == 1)
            {
                DirectionEnum dirEnum = Direction.GetDirection(dir);
                MapManager.Instance.TryGetBuilding(_prevPosition, out Building prevBuilding);
                MapManager.Instance.TryGetBuilding(tilePosition, out Building curBuilding);
                if (prevBuilding != null) prevBuilding.SetRotation(dirEnum);
                if (curBuilding != null) curBuilding.SetRotation(dirEnum);
                _curDirection = dirEnum;
            }

            _prevPosition = tilePosition;
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

        private bool CheckResource(BuildingSO buildingSO)
        {
            bool canInsert = true;

            foreach (Resource resource in buildingSO.needResource)
            {
                bool exsist = _playerItemCollector.IsHaveItem((int)resource.type);

                if (!exsist)
                {
                    canInsert = false;
                    break;
                }

                int amount = _playerItemCollector.GetItemAmount((int)resource.type);

                if (amount < resource.amount)
                {
                    canInsert = false;
                    break;
                }
            }

            return canInsert;
        }

        private void SetBulidingPreview(BuildingSO buildingSO)
        {
            Vector2 position = Input.mousePosition;
            int size = _buildingSet.FindBuilding(_buildTarget).tileSize;

            _buildingPreview.UpdateBuildidng(position, size);
            if (buildingSO.canRotate) _buildingPreview.SetDirection(_curDirection);
            else _buildingPreview.DisableDirection();
        }

        private void RotateBuilding(BuildingSO buildingSO)
        {
            if (buildingSO.canRotate == false) return;

            if (Input.GetKeyDown(KeyCode.R))
                _curDirection = (DirectionEnum)(((int)_curDirection + 1) % 4);
        }
    }
}