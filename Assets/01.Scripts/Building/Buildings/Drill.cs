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

            _resources.ForEach(r => _container.Add(new Resource(r, _miningCnt)));
        }
    }

    // 건물을 짓는 순간에 드릴이 채굴할 수 있는 자원은 정해져 있기 때문에 여기서 지정해주
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
