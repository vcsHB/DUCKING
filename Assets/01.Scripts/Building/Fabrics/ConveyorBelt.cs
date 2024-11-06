using ItemSystem;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class ConveyorBelt : Fabric, IResourceInput, IResourceOutput
{
    private Resource _container;

    public void TransferResource()
    {
        Vector2Int direction = Direction.GetTileDirection(_direction);

    }

    public bool TryInsertResource(Resource resource, out Resource remain)
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

        remain = new Resource();
        remain.type = ResourceType.None;
        return true;
    }
}
