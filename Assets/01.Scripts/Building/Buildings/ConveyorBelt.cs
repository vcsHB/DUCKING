using BuildingManage;
using ResourceSystem;
using UnityEngine;

public class ConveyorBelt : Building, IResourceInput, IResourceOutput
{
    [SerializeField] private ConveyorBeltResource _beltResource;

    [SerializeField] private Resource _container;
    [SerializeField] private float _speed = 2f;
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

            if (_container.type == ResourceType.None)
                _beltResource.DisableResource();
        }
        else
        {
            _process += Time.deltaTime * _speed;
            _beltResource?.Move(_process);
        }
    }

    public void TransferResource()
    {
        Vector2Int nextPosition = Position.min + Direction.GetTileDirection(_direction);
        bool buildingExsist =
            MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);

        if (!buildingExsist || !connectedBuilding.TryGetComponent(out IResourceInput input)) return;

        DirectionEnum opposite = Direction.GetOpposite(_direction);
        input.TryInsertResource(_container, opposite, out _container);
        if (_container.type == ResourceType.None) _process = 0;
    }

    public bool TryInsertResource(Resource resource, DirectionEnum direction, out Resource remain)
    {
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

        _beltResource.gameObject.SetActive(true);

        Vector2 from = Position.center + (Vector2)Direction.GetTileDirection(direction) / 2;
        Vector2 to = Position.center + (Vector2)Direction.GetTileDirection(_direction) / 2;
        _beltResource.Init(Position.center, from, to, resource);

        remain = new Resource();
        remain.type = ResourceType.None;
        return true;
    }
}
