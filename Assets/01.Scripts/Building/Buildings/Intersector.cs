using BuildingManage;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Intersector : Transfortation
{
    [SerializeField] private float _speed;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Update()
    {
        for (int i = 0; i < _processes.Count; i++)
        {
            if (_container[i].type == ResourceType.None) continue;

            if (_processes[i] >= 1)
            {
                _processes[i] = 1;
                TransferResource();
            }
            else
            {
                _processes[i] += Time.deltaTime * _speed;
            }
        }
    }

    public override void Build(Vector2Int position, DirectionEnum direction, bool save = false)
    {
        base.Build(position, direction, save);

        _inputDirection.Clear();
        _outputDirection.Clear();
        _container.Clear();
        _processes.Clear();

        for (int i = 0; i < 2; i++)
        {
            _inputDirection.Add(DirectionEnum.None);
            _outputDirection.Add(DirectionEnum.None);
            _container.Add(new Resource(ResourceType.None, 0));
            _processes.Add(0);
        }

        CheckNeighbor(position);
    }

    public override void TransferResource()
    {
        for(int i = 0; i < 2; i++)
        {
            DirectionEnum dir = _outputDirection[i];
            Vector2Int nextPosition = Position.min + Direction.GetTileDirection(dir);

            bool buildingExsist =
                MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);

            if (!connectedBuilding.TryGetComponent(out IResourceInput input)) return;

            DirectionEnum opposite = Direction.GetOpposite(dir);

            if (_processes[i] < 1) continue;
            
            input.TryInsertResource(_container[i], opposite, out Resource remain);
            
            if (remain.type == ResourceType.None)
            {
                _container[i] = remain;
                _processes[i] = 0;
            }
            else
            {
                _container[i] = remain;
            }
        }


        //if (_process >= 1)
        //{
        //    Vector2Int nextPosition = Position.min + Direction.GetTileDirection(_outputDirection[0]);
        //    bool buildingExist = MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);
        //    if (buildingExist)
        //    {
        //        if (connectedBuilding.TryGetComponent(out IResourceInput input))
        //        {
        //            DirectionEnum opposite = Direction.GetOpposite(_outputDirection[0]);
        //            input.TryInsertResource(_container, opposite, out _container);
        //        }
        //    }
        //}

        //if (_secondaryProcess >= 1)
        //{
        //    Vector2Int nextPosition = Position.min + Direction.GetTileDirection(_outputDirection[1]);
        //    bool buildingExist = MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);
        //    if (buildingExist)
        //    {
        //        if (connectedBuilding.TryGetComponent(out IResourceInput input))
        //        {
        //            DirectionEnum opposite = Direction.GetOpposite(_outputDirection[1]);
        //            input.TryInsertResource(_secondaryContainer, opposite, out _secondaryContainer);
        //        }
        //    }
        //}

        //if (_container.type == ResourceType.None) _process = 0;
        //if (_secondaryContainer.type == ResourceType.None) _secondaryProcess = 0;
    }

    public override bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
    {
        base.TryInsertResource(resource, inputDir, out remain);
        if (!_inputDirection.Contains(inputDir))
        {
            remain = resource;
            return false;
        }

        int idx = _inputDirection.IndexOf(inputDir);

        if (_container[idx].type  != ResourceType.None)
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
