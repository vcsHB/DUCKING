using BuildingManage;
using ResourceSystem;
using System.Collections.Generic;
using UnityEngine;

public abstract class Source : Building, IResourceOutput
{
    protected List<Vector2Int> _connectedPositions = new List<Vector2Int>();
    protected List<(IResourceInput, DirectionEnum)> _connectedInputs = new();
    protected List<Resource> _container = new List<Resource>();
    [SerializeField] protected float _transferDelay = 1;
    protected float _prevTransfer;

    protected virtual void OnEnable()
    {
        MapManager.Instance.BuildController.OnBuildingChange += UpdateConnectedInput;
    }

    protected virtual void OnDisable()
    {
        if (!MapManager.IsDestroyed)
            MapManager.Instance.BuildController.OnBuildingChange -= UpdateConnectedInput;
    }


    public virtual void TransferResource()
    {
        if (_container.Count <= 0 || _prevTransfer + _transferDelay > Time.time) return;

        for (int j = 0; j < _connectedInputs.Count; j++)
        {
            for (int i = 0; i < _container.Count; i++)
            {
                _prevTransfer = Time.time;
                var input = _connectedInputs[j];

                ResourceType type = _container[i].type;
                Resource resource = new Resource(type, 1);
                Resource containerResource = new Resource(type, _container[i].amount - 1);

                bool insertable = input.Item1.TryInsertResource
                    (resource, Direction.GetOpposite(input.Item2), out resource);

                if (insertable)
                {
                    if (containerResource.amount > 1) _container[i] = containerResource;
                    else
                    {
                        _container.RemoveAt(i--);
                        break;
                    }
                }
                break;
            }
        }
    }

    protected virtual void UpdateConnectedInput()
    {
        _connectedInputs.Clear();

        for (int i = 0; i < _connectedPositions.Count; i++)
        {
            bool buildingExsist =
                MapManager.Instance.TryGetBuilding(_connectedPositions[i], out Building building);

            if (!buildingExsist || !building.TryGetComponent(out IResourceInput input)) continue;

            DirectionEnum direction = Direction.GetDirection(Position.min, Position.max, _connectedPositions[i]);
            _connectedInputs.Add((input, direction));
        }
    }

    protected override void SetPosition(Vector2Int position)
    {
        base.SetPosition(position);

        for (int i = Position.min.x; i <= Position.max.x; i++)
        {
            _connectedPositions.Add(new Vector2Int(i, Position.min.y - 1));
            _connectedPositions.Add(new Vector2Int(i, Position.max.y + 1));
        }
        for (int i = Position.min.y; i <= Position.max.y; i++)
        {
            _connectedPositions.Add(new Vector2Int(Position.min.x - 1, i));
            _connectedPositions.Add(new Vector2Int(Position.max.x + 1, i));
        }

        UpdateConnectedInput();
    }
}
