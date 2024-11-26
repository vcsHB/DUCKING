using BuildingManage;
using ResourceSystem;
using TMPro;
using UnityEngine;

public class Distributor : Transfortation
{
    [SerializeField] private float _speed;
    private int _curOutput = 0;

    private void Update()
    {
        for (int i = 0; i < _processes.Count; i++)
        {
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

        for (int j = 0; j < _processes.Count; j++)
        {
            if (_processes[j] < 1) continue;

            input.TryInsertResource(_container[j], opposite, out Resource remain);
            if (remain.type == ResourceType.None || remain.amount <= 0)
            {
                _processes.RemoveAt(j);
                _container.RemoveAt(j);
                j--;
            }
            else
            {
                _container[j] = remain;
            }
            break;
        }

        //base.TransferResource();
    }

    public override bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
    {
        return base.TryInsertResource(resource, inputDir, out remain);
    }

    protected override void CheckNeighbor(Vector2Int position)
    {
        if (_inputDirection.Count > 0) return;

        _inputDirection.Clear();
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
