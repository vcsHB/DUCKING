using BuildingManage;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Intersector : Transfortation
{
    private Resource _secondaryContainer;
    [SerializeField] private float _speed;
    private float _process = 0;
    private float _secondaryProcess = 0;

    protected override void OnEnable()
    {
        base.OnEnable();
        _secondaryContainer = new Resource(ResourceType.None, 0);
    }

    private void Update()
    {
        if (_container.type == ResourceType.None)
        {
            _process = 0;
        }
        else
        {
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

        if (_secondaryContainer.type == ResourceType.None)
        {
            _secondaryProcess = 0;
        }
        else
        {
            if (_secondaryProcess >= 1)
            {
                _secondaryProcess = 1;
                TransferResource();
            }
            else
            {
                _secondaryProcess += Time.deltaTime * _speed;
            }
        }


    }

    public override void Build(Vector2Int position, DirectionEnum direction, bool save = false)
    {
        base.Build(position, direction, save);

        MapManager.Instance.TryGetBuilding(position, out Building buildingInstance);
        Intersector intersector = (buildingInstance as Intersector);

        intersector._inputDirection.Clear();
        intersector._outputDirection.Clear();
        for (int i = 0; i < 2; i++)
        {
            intersector._inputDirection.Add(DirectionEnum.None);
            intersector._outputDirection.Add(DirectionEnum.None);
        }

        intersector.CheckNeighbor(position);
    }

    public override void TransferResource()
    {
        if (_process >= 1)
        {
            Vector2Int nextPosition = Position.min + Direction.GetTileDirection(_outputDirection[0]);
            bool buildingExist = MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);
            if (buildingExist)
            {
                if (connectedBuilding.TryGetComponent(out IResourceInput input))
                {
                    DirectionEnum opposite = Direction.GetOpposite(_outputDirection[0]);
                    input.TryInsertResource(_container, opposite, out _container);
                }
            }
        }

        if (_secondaryProcess >= 1)
        {
            Vector2Int nextPosition = Position.min + Direction.GetTileDirection(_outputDirection[1]);
            bool buildingExist = MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);
            if (buildingExist)
            {
                if (connectedBuilding.TryGetComponent(out IResourceInput input))
                {
                    DirectionEnum opposite = Direction.GetOpposite(_outputDirection[1]);
                    input.TryInsertResource(_secondaryContainer, opposite, out _secondaryContainer);
                }
            }
        }

        if (_container.type == ResourceType.None) _process = 0;
        if (_secondaryContainer.type == ResourceType.None) _secondaryProcess = 0;
    }

    public override bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
    {
        if (!_inputDirection.Contains(inputDir))
        {
            remain = resource;
            return false;
        }

        int idx = _inputDirection.IndexOf(inputDir);

        if (idx == 0)
        {
            if (_container.type != ResourceType.None)
            {
                remain = resource;
                return false;
            }

            remain = new Resource(ResourceType.None, 0);
            _container = resource;
            return true;
        }
        else if (idx == 1)
        {
            if (_secondaryContainer.type != ResourceType.None)
            {
                remain = resource;
                return false;
            }

            remain = new Resource(ResourceType.None, 0);
            _secondaryContainer = resource;
            return true;
        }

        remain = resource;
        return false;
    }

    protected override void CheckNeighbor(Vector2Int position)
    {
        if (_inputDirection == null || _outputDirection == null ||
            _inputDirection.Count < 2 || _outputDirection.Count < 2) return;

        for (int i = 0; i < 4; i++)
        {
            DirectionEnum direction = (DirectionEnum)i;

            if (_inputDirection.Contains(direction) ||
                _inputDirection.Contains(Direction.GetOpposite(direction))) continue;

            Vector2Int connected = position + Direction.GetTileDirection(direction);

            MapManager.Instance.TryGetBuilding(position, out Building belt);
            ConveyorBelt beltInstance = belt as ConveyorBelt;

            bool buildingExsist =
                MapManager.Instance.TryGetBuilding(connected, out Building building);

            if (buildingExsist)
            {
                if (building is Transfortation transfortation)
                {
                    if (!transfortation.ContainOutput(Direction.GetOpposite((DirectionEnum)i))
                        || ContainOutput((DirectionEnum)i)) continue;

                    SetInputDirection((DirectionEnum)i);
                    break;
                }
                else if (building.TryGetComponent(out IResourceOutput output))
                {
                    beltInstance.SetInputDirection((DirectionEnum)i);
                    break;
                }
            }
        }

        for (int i = 0; i < _inputDirection.Count; i++)
        {
            if (_inputDirection[i] != DirectionEnum.None)
                _outputDirection[i] = Direction.GetOpposite(_inputDirection[i]);
        }
    }

    public override void SetInputDirection(DirectionEnum inputDir)
    {
        if (_inputDirection.Contains(inputDir)) return;

        if (_inputDirection[0] == DirectionEnum.None) _inputDirection[0] = inputDir;
        else if (_inputDirection[1] == DirectionEnum.None) _inputDirection[1] = inputDir;
    }

    public override void RemoveInputDirection(DirectionEnum directionEnum)
    {
        if (_inputDirection.Contains(directionEnum))
        {
            int idx = _inputDirection.IndexOf(directionEnum);
            _inputDirection[idx] = DirectionEnum.None;
        }
    }
}
