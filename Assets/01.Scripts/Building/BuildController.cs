using System;
using System.Collections.Generic;
using AgentManage.PlayerManage;
using InputManage;
using ItemSystem;
using ResourceSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace BuildingManage
{
    public class BuildController : MonoBehaviour
    {
        public event Action OnBuildingChange;

        private BuildingSetSO _buildingSet;
        private Transform _buildingParent;

        private BuildingEnum _buildTarget;
        private DirectionEnum _curDirection;
        [SerializeField] private UIInputReaderSO _inputReader;
        [SerializeField] private BuildingPreview _buildingPreview;
        [SerializeField] private PlayerItemCollector _playerItemCollector;
        [SerializeField] private List<BuildingEnum> _autoRotateBuildings;

        private Vector2Int _prevPosition = new Vector2Int(int.MinValue, int.MinValue);
        private bool _isBuilding = false;
        private bool _tryBuild;

        private void OnEnable()
        {
            _inputReader.LeftClickEvent += OnBuild;
        }
        
        private void OnDisable()
        {
            _inputReader.LeftClickEvent -= OnBuild;
        }

        private void Update()
        {
            Vector2Int tilePosition
                = MapManager.Instance.GetTilePos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            bool pointerOverlapUI = EventSystem.current.IsPointerOverGameObject();

            if(pointerOverlapUI)  _isBuilding = false;

            //지워주는 친구
            if (Input.GetMouseButton(1) && !pointerOverlapUI) TryDestroyBuilding(tilePosition);

            if (!_tryBuild || _buildTarget == BuildingEnum.None) return;

            BuildingSO buildingSO = _buildingSet.FindBuilding(_buildTarget);

            bool canBuild = CheckResource(buildingSO);
            SetBulidingPreview(buildingSO, canBuild);
            RotateBuilding(buildingSO);

            if (!canBuild) return;

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

            if (_autoRotateBuildings.Contains(_buildTarget))
                AutoRotate(position);

            DirectionEnum direction = info.canRotate ? _curDirection : DirectionEnum.Down;
            info.building.Build(position, direction, save);
            OnBuildingChange?.Invoke();

            foreach (Resource resource in info.needResource)
            {
                //재료 소모하는 부분
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


        private void OnBuild(bool btnDown)
        {
            if (!_tryBuild || _buildTarget == BuildingEnum.None) return;

            bool pointerOverlapUI = EventSystem.current.IsPointerOverGameObject();

            if (!pointerOverlapUI)
            {
                if (Input.GetMouseButtonDown(0)) _isBuilding = true;
                if (Input.GetMouseButtonUp(0))
                {
                    _prevPosition = new Vector2Int(int.MinValue, int.MinValue);
                    _isBuilding = false;
                }
            }
        }
        private void AutoRotate(Vector2Int tilePosition)
        {
            Vector2Int dir = tilePosition - _prevPosition;

            if (Mathf.Abs(dir.x + dir.y) == 1)
            {
                DirectionEnum dirEnum = Direction.GetDirection(dir);
                MapManager.Instance.TryGetBuilding(_prevPosition, out Building prevBuilding);
                if (prevBuilding != null) prevBuilding.SetRotation(dirEnum);
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

        private void SetBulidingPreview(BuildingSO buildingSO, bool canBuild)
        {
            Vector2 position = Input.mousePosition;
            int size = _buildingSet.FindBuilding(_buildTarget).tileSize;

            _buildingPreview.UpdateBuildidng(position, size, !canBuild);

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