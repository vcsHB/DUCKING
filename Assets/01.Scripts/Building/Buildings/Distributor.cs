using BuildingManage;
using ResourceSystem;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class Distributor : Transfortation
{
    [SerializeField] private float _speed;

    private Resource _container = new Resource(ResourceType.None, 0);
    private float _process = 0;
    private int _curOutput = 0;

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
        _curOutput = (_curOutput + 1) % 3;

        DirectionEnum dir = _outputDirection[_curOutput];
        Vector2Int nextPosition = Position.min + Direction.GetTileDirection(dir);

        bool buildingExsist =
            MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);

        if (!buildingExsist || !connectedBuilding.TryGetComponent(out IResourceInput input)) return;

        //방향을 반대로 돌려서 input에 TryInsertResource를 호출해줘
        DirectionEnum opposite = Direction.GetOpposite(dir);

        input.TryInsertResource(_container, opposite, out Resource remain);
        if (remain.type == ResourceType.None || remain.amount <= 0)
        {
            _process = 0;
            _container = new Resource(ResourceType.None, 0);
        }
        else
        {
            _container = remain;
        }

        //base.TransferResource();
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
        _container = resource;
        remain = new Resource(ResourceType.None, 0);
        return true;
    }

    protected override void CheckNeighbor(Vector2Int position)
    {
        //이미 인풋이 있다면
        if (_inputDirection.Count > 0) return;

        for (int i = 0; i < 4; i++)
        {
            Vector2Int connected = position + Direction.directionsInt[i];
            MapManager.Instance.TryGetBuilding(position, out Building belt);
            //ConveyorBelt beltInstance = belt as ConveyorBelt;

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
                    SetInputDirection((DirectionEnum)i);
                    break;
                }
            }
        }

        if (_inputDirection.Count > 0)
        {
            _outputDirection.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (i == (int)_inputDirection[0]) continue;

                _outputDirection.Add((DirectionEnum)i);
            }
        }
    }
}
