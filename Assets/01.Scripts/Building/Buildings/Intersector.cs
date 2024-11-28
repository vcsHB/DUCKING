using BuildingManage;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Intersector : Transfortation
{
    [SerializeField] private float _speed;

    private Resource[] _container = new Resource[2] { new Resource(ResourceType.None, 0), new Resource(ResourceType.None, 0) };
    private float[] _process = { 0, 0 };


    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            if (_container[i].type == ResourceType.None || _container[i].amount <= 0) 
                continue;

            if (_process[i] >= 1)
            {
                _process[i] = 1;
                TransferResource();
            }
            else
            {
                _process[i] += Time.deltaTime * _speed;
            }

        }
    }

    public override void Build(Vector2Int position, DirectionEnum direction, bool save = false)
    {
        base.Build(position, direction, save);

        _inputDirection.Clear();
        _outputDirection.Clear();

        for (int i = 0; i < 2; i++)
        {
            _inputDirection.Add(DirectionEnum.None);
            _outputDirection.Add(DirectionEnum.None);
        }

        CheckNeighbor(position);
    }

    public override void TransferResource()
    {
        for (int i = 0; i < 2; i++)
        {
            DirectionEnum dir = _outputDirection[i];
            Vector2Int nextPosition = Position.min + Direction.GetTileDirection(dir);

            bool buildingExsist = 
                MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);

            if (!connectedBuilding.TryGetComponent(out IResourceInput input)) return;

            DirectionEnum opposite = Direction.GetOpposite(dir);

            if (_process[i] < 1) continue;

            input.TryInsertResource(_container[i], opposite, out Resource remain);

            if (remain.type == ResourceType.None)
            {
                _container[i] = remain;
                _process[i] = 0;
            }
            else
            {
                _container[i] = remain;
            }
        }
    }

    public override bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
    {
        //base.TryInsertResource(resource, inputDir, out remain);
        if (!_inputDirection.Contains(inputDir))
        {
            remain = resource;
            return false;
        }

        int idx = _inputDirection.IndexOf(inputDir);

        if (_container[idx].type != ResourceType.None)
        {
            remain = resource;
            return false;
        }

        remain = remain = new Resource(ResourceType.None, 0);
        _container[idx] = resource;
        return true;
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
