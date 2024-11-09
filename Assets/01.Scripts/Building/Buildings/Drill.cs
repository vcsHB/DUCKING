using BuildingManage;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Drill : Factory, IMineable
{
    [SerializeField] private ResourceInfoGroupSO _resourceGroup;
    [SerializeField] private float _miningTime = 3f;
    [SerializeField] private int _miningCnt = 2;

    private List<ResourceType> _resources;

    private int _resourceCnt;
    private float _curMiningTime = 0f;
    private Tilemap _resourceTilemap;

    private void Update()
    {
        Mine();
        TransferResource();
    }

    public void Mine()
    {
        if (_container.Count > 0) return;

        _curMiningTime += Time.deltaTime;

        if (_curMiningTime >= _miningTime)
        {
            _curMiningTime = 0f;
            _container = new List<Resource>();

            _resources.ForEach(r =>
            {
                _container.Add(new Resource(r, _miningCnt));
            });
        }
    }

    //public void TransferResource()
    //{
    //    for (int i = 0; i < _nextPositions.Count; i++)
    //    {
    //        Vector2Int np = _nextPositions[i];

    //        bool buildingExsist =
    //            MapManager.Instance.TryGetBuilding(np, out Building connectedBuilding);

    //        if (!buildingExsist || !connectedBuilding.TryGetComponent(out IResourceInput input)) continue;

    //        foreach (var resourceType in _resources)
    //        {
    //            if (_container[resourceType].amount <= 0) continue;

    //            Resource resource = _container[resourceType];
    //            resource.amount = 1;

    //            DirectionEnum opposite = Direction.GetOpposite(Direction.GetDirection(Position.min, Position.max, np));
    //            input.TryInsertResource(resource, opposite, out resource);

    //            Resource remain =
    //                new Resource(resourceType, _container[resourceType].amount - 1);
    //            if (resource.type != ResourceType.None) remain.amount++;

    //            _container[resourceType] = remain;
    //        }
    //    }
    //}

    protected override void SetPosition(Vector2Int position)
    {
        base.SetPosition(position);

        _resourceTilemap = MapManager.Instance.ResourceTile;
        _resources = new List<ResourceType>();
        _curMiningTime = 0;

        //리소스 종류,컨테이너 Init
        for (int i = Position.min.x; i <= Position.max.x; i++)
        {
            for (int j = Position.min.y; j <= Position.max.y; j++)
            {
                TileBase resourceTile = _resourceTilemap.GetTile(new Vector3Int(i, j));
                if (resourceTile == null) continue;

                ResourceType resourceType = _resourceGroup.GetResourceType(resourceTile);
                _resources.Add(resourceType);
            }
        }
    }
}
