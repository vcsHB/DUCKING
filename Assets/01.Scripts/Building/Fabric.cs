using AgentManage;
using BuildingManage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fabric : MonoBehaviour, IBuildable, IDamageable
{
    [SerializeField] protected FabricSO _buildingInfo;
    protected DirectionEnum _direction;

    protected int _health;

    public Vector2Int Position { get; protected set; }
    public FabricSO BuildingInfo => _buildingInfo;
    public FabricEnum BuildingType => _buildingInfo.fabricType;

    private void Awake()
    {
        _health = _buildingInfo.health;
    }

    public bool CheckPosition(Vector2Int pos)
    {
        int size = Mathf.CeilToInt(_buildingInfo.tileSize / 2);

        if (Mathf.Abs(pos.x - Position.x) < size && Mathf.Abs(pos.y - Position.y) < size)
            return true;

        return false;
    }

    public void SetPosition(Vector2Int position) => Position = position;

    public virtual void ApplyDamage(int amount)
    {
        _health -= amount;

        if(_health <= 0)
        {
            _health = 0;
            Destroy();
        }
    }

    public void Destroy()
    {

    }

    public void ReadyDestroy()
    {

    }

    public void Build(Vector2Int position, DirectionEnum direction)
    {
        Position = position;
        Vector2 worldPos = MapManager.Instance.GetWorldPos(position);
        Quaternion rotation = Quaternion.Euler(Direction.GetDirection(direction));

        Instantiate(gameObject, worldPos, rotation);
        MapManager.Instance.AddBuilding(this, false);
    }
}
