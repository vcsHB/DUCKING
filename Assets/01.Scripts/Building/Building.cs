using AgentManage;
using BuildingManage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IBuildable, IDamageable
{
    [SerializeField] protected BuildingSO buildingInfo;
    protected DirectionEnum direction;

    public BuildingSO BuildingInfo => buildingInfo;

    public void ApplyDamage(int amount)
    {
        
    }

    public void Build(Vector2 position)
    {
        //Vector2 tilePos = MapManager.Instance.RoundToTilePos(position);
        //Vector3 rotation = Direction.GetDirection(direction);

        //Instantiate(buildingInfo.prefab, tilePos, Quaternion.Euler(rotation));
    }

    public void Destroy()
    {

    }

    public void ReadyDestroy()
    {

    }
}
