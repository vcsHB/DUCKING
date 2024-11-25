using BuildingManage;
using ResourceSystem;
using UnityEngine;

public class Distributor : Transfortation
{
    [SerializeField] private float _speed;
    private float _process = 0;

    private void Update()
    {
        if (_container.type == ResourceType.None)
        {
            _process = 0;
            return;
        }

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
        base.TransferResource();
        if (_container.type == ResourceType.None) _process = 0;
    }



    protected override void CheckNeighbor(Vector2Int position)
    {
        _inputDirection.Clear();
        //if (_inputDirection.Count > 1) return;

        _inputDirection.Clear();
        for (int i = 0; i < 4; i++)
        {
            Vector2Int connected = position + Direction.directionsInt[i];
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
