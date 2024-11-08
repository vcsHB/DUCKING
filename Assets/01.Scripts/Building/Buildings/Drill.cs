using BuildingManage;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Drill : Building, IMineable
{
    [SerializeField] private ResourceInfoGroupSO _resourceGroup;
    [SerializeField] private float _miningTime = 3f;
    [SerializeField] private int _miningCnt = 2;

    private List<ResourceType> _resources;
    private List<Vector2Int> _nextPositions;
    private Dictionary<ResourceType, Resource> _container;
    private Dictionary<ResourceType, int> _maximum;
    private float _curMiningTime = 0f;
    private Tilemap _resourceTilemap;

    private void Update()
    {
        Mine();
        TransferResource();
    }

    public void Mine()
    {
        _curMiningTime += Time.deltaTime;

        if (_curMiningTime >= _miningTime)
        {
            _curMiningTime = 0f;
            _resources.ForEach(r =>
            {
                if (_container[r].amount < _maximum[r])
                {
                    int amount = _container[r].amount + _miningCnt;
                    Resource resource = new Resource(r, amount);
                    _container[r] = resource;
                }
            });
        }
    }

    public void TransferResource()
    {
        for (int i = 0; i < _nextPositions.Count; i++)
        {
            Vector2Int np = _nextPositions[i];

            bool buildingExsist =
                MapManager.Instance.TryGetBuilding(np, out Building connectedBuilding);

            if (!buildingExsist || !connectedBuilding.TryGetComponent(out IResourceInput input)) continue;

            foreach (var resourceType in _resources)
            {
                if (_container[resourceType].amount <= 0) continue;

                Resource resource = _container[resourceType];
                resource.amount = 1;

                DirectionEnum opposite = Direction.GetOpposite(Direction.GetDirection(Position.min, Position.max, np));
                input.TryInsertResource(resource, opposite, out resource);

                Resource remain =
                    new Resource(resourceType, _container[resourceType].amount - 1);
                if (resource.type != ResourceType.None) remain.amount++;

                _container[resourceType] = remain;
            }
        }
    }

    protected override void SetPosition(Vector2Int position)
    {
        base.SetPosition(position);

        _resourceTilemap = MapManager.Instance.ResourceTile;
        _container = new Dictionary<ResourceType, Resource>();
        _maximum = new Dictionary<ResourceType, int>();
        _nextPositions = new List<Vector2Int>();
        _resources = new List<ResourceType>();
        _curMiningTime = 0;

        for (int i = Position.min.x; i <= Position.max.x; i++)
        {
            for (int j = Position.min.y; j <= Position.max.y; j++)
            {
                TileBase resourceTile = _resourceTilemap.GetTile(new Vector3Int(i, j));
                if (resourceTile == null) continue;

                ResourceType resourceType = _resourceGroup.GetResourceType(resourceTile);
                if (!_container.ContainsKey(resourceType))
                {
                    _container.Add(resourceType, new Resource(resourceType, 0));
                    _maximum.Add(resourceType, 0);
                }
                _maximum[resourceType] += _miningCnt;
                _resources.Add(resourceType);
            }
        }

        for (int i = Position.min.x; i <= Position.max.x; i++)
        {
            _nextPositions.Add(new Vector2Int(i, Position.min.y - 1));
            _nextPositions.Add(new Vector2Int(i, Position.max.y + 1));
        }
        for (int i = Position.min.y; i <= Position.max.y; i++)
        {
            _nextPositions.Add(new Vector2Int(Position.min.x - 1, i));
            _nextPositions.Add(new Vector2Int(Position.max.x + 1, i));
        }
        Debug.Log(_container.Count);
        Debug.Log(_resources.Count);
        Debug.Log(_nextPositions.Count);
    }
}
