using BuildingManage;
using ResourceSystem;
using UnityEngine;

public class ConveyorBelt : Building, IResourceInput, IResourceOutput
{
    [SerializeField] private Resource _container;
    [SerializeField] private float _speed = 2f;
    private float _process = 0;

    private Transform _resourceTrm;

    private void Update()
    {
        if(_container.type == ResourceType.None)
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

    public void TransferResource()
    {
        Vector2Int nextPosition = Position.min + Direction.GetTileDirection(_direction);
        bool buildingExsist =
            MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);

        if (!buildingExsist || !connectedBuilding.TryGetComponent(out IResourceInput input)) return;

        bool canInsert = input.TryInsertResource(_container, out _container);
        if (canInsert) _process = 0;
    }

    public bool TryInsertResource(Resource resource, out Resource remain)
    {
        Resource
        //리소스가 이미 존재할 때
        if (_container.type != ResourceType.None)
        {
            if (_container.type == resource.type)
            {
                _container.amount += resource.amount;
            }
            else
            {
                remain = resource;
                return false;
            }
        }
        else
        {
            _container = resource;
        }

        remain = new Resource();
        remain.type = ResourceType.None;
        return true;
    }
}
