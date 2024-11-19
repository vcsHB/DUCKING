using System;
using AgentManage.PlayerManage;
using UnityEngine;

namespace BuildingManage
{
    public class BuildController : MonoBehaviour
    {
        public event Action OnBuildingChange;

        private BuildingSetSO _buildingSet;
        private Transform _buildingParent;

        [SerializeField] private BuildingEnum _buildTarget;
        [SerializeField] private DirectionEnum _curDirection;
        [SerializeField] private BuildingPreview _buildingPreview;
        [SerializeField] private PlayerItemCollector _playerItemCollector;

        private Vector2Int _prevPosition = new Vector2Int(int.MinValue, int.MinValue);
        private bool _isBuilding = false;
        private bool _tryBuild;

        private void Update()
        {
            Vector2Int tilePosition
                = MapManager.Instance.GetTilePos(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (Input.GetMouseButton(1)) TryDestroyBuilding(tilePosition);
            if (Input.GetKeyDown(KeyCode.P))
            {
                _tryBuild = !_tryBuild;
                SetPreview(_tryBuild);
            }

            if (!_tryBuild) return;

            Vector2 position = Input.mousePosition;
            int size = _buildingSet.FindBuilding(_buildTarget).tileSize;

            _buildingPreview.UpdateBuildidng(position, size);

            

            if (Input.GetMouseButtonDown(0)) _isBuilding = true;
            if (Input.GetMouseButtonUp(0))
            {
                _prevPosition = new Vector2Int(int.MinValue, int.MinValue);
                _isBuilding = false;
            }

            if (_isBuilding)
            {
                bool buildThisFrame = TryBuild(_buildTarget, tilePosition, true);

                if (buildThisFrame)
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
            }

            //회전을 하다!
            if (Input.GetKeyDown(KeyCode.R))
                _curDirection = (DirectionEnum)(((int)_curDirection + 1) % 4);
        }

        public void SetBuildTarget(BuildingEnum buildingType)
        {
            _buildTarget = buildingType;
            _tryBuild = true;
            SetPreview(true);
        }

        public void HandleDisableBuildMode()
        {
            _tryBuild = false;
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

            return true;
        }

        public void TryDestroyBuilding(Vector2Int position)
        {
            HandleDisableBuildMode();
            bool buildingExist =
                        MapManager.Instance.TryGetBuilding(position, out Building building);

            if (buildingExist)
            {
                OnBuildingChange?.Invoke();
                building.Destroy();
            }
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

        private bool CheckResourceEnough()
        {
            return false;
        }
    }
}

