using ItemSystem;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingManage
{
    public class LogicConveyorBelt : Transfortation
    {
        public ConveyorLogicSO logicSO;
        [SerializeField] private float _speed = 3;

        private Resource _container = new Resource(ResourceType.None, 0);
        private float _process = 0;
        private bool _isLeft = false;

        public void SetLogic(ConveyorLogicSO logic)
            => logicSO = logic;

        private void Update()
        {
            if (_container.type == ResourceType.None || _container.amount <= 0) return;

            if (_process >= 1)
            {
                _process = 1;
                TransferResource();
            }
            else
            {
                _process += Time.deltaTime * _speed;
            }
        }

        public override void TransferResource()
        {
            DirectionEnum direction = _outputDirection[0];

            if (logicSO.CheckLogic(_container.type) == false)
            {
                direction = _outputDirection[_isLeft ? 1 : 2];
                _isLeft = !_isLeft;
            }

            Vector2Int nextPosition = Position.min + Direction.GetTileDirection(direction);

            bool buildingExsist =
                MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);

            if (buildingExsist)
            {
                if (!connectedBuilding.TryGetComponent(out IResourceInput input)) return;

                input.TryInsertResource(_container, Direction.GetOpposite(direction), out Resource remain);
                if (remain.type == ResourceType.None)
                {
                    _process = 0;
                    _container = new Resource(ResourceType.None, 0);
                }
            }
        }

        public override bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
        {
            if (!_inputDirection.Contains(inputDir)
                || _container.type != ResourceType.None || _container.amount > 0)
            {
                remain = resource;
                return false;
            }

            _process = 0;
            _container = new Resource(resource.type, 1);

            if (resource.amount > 1)
            {
                remain = new Resource(resource.type, resource.amount - 1);
                return false;
            }

            remain = new Resource(ResourceType.None, 0);
            return true;
        }

        protected override void CheckNeighbor(Vector2Int position)
        {
            DirectionEnum inputDirection = DirectionEnum.Down;

            if (_inputDirection.Count > 0)
            {
                inputDirection = _inputDirection[0];
                SetRotation(Direction.GetOpposite(inputDirection));
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                Vector2Int connectedPos = position + Direction.directionsInt[i];

                bool buildingExsist =
                    MapManager.Instance.TryGetBuilding(connectedPos, out Building building);

                if (!buildingExsist) continue;

                if (building is Transfortation transfortation)
                {
                    //연결된놈, 현재 컨베이어 벨트의 방향확인
                    if (!transfortation.ContainOutput(Direction.GetOpposite((DirectionEnum)i))
                        || ContainOutput((DirectionEnum)i)) continue;

                    _inputDirection.Add((DirectionEnum)i);
                    break;
                }
                else if (building.TryGetComponent(out IResourceOutput output))
                {
                    if (ContainOutput((DirectionEnum)i)) continue;

                    _inputDirection.Add((DirectionEnum)i);
                    break;
                }
            }

            if (_inputDirection.Count > 0)
                inputDirection = _inputDirection[0];

            SetRotation(Direction.GetOpposite(inputDirection));
        }

        public override void SetRotation(DirectionEnum direction)
        {
            _direction = direction;

            _outputDirection.Clear();
            _outputDirection.Add(direction);
            _outputDirection.Add((DirectionEnum)(((int)direction + 1) % 4));
            _outputDirection.Add((DirectionEnum)(((int)direction + 3) % 4));

            _inputDirection.Clear();
            _inputDirection.Add(Direction.GetOpposite(direction));

        }
    }
}

