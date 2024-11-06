using AgentManage;
using BuildingManage;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour, IBuildable, IDamageable
{
    [SerializeField] protected BuildingSO _buildingInfo;
    protected DirectionEnum _direction;

    protected int _health;

    public BuildingSize Position { get; protected set; }
    public BuildingSO BuildingInfo => _buildingInfo;
    public BuildingEnum BuildingType => _buildingInfo.fabricType;

    private void Awake()
    {
        _health = _buildingInfo.health;
    }

    public bool CheckPosition(Vector2Int pos) => Position.IsOverlap(pos);
    public bool CheckPosition(BuildingSize pos) => Position.IsOverlap(pos);

    public void SetPosition(Vector2Int position)
    {
        Position = new BuildingSize(position, _buildingInfo.tileSize);
    }

    public virtual void ApplyDamage(int amount)
    {
        _health -= amount;

        if (_health <= 0)
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

    public void Build(Vector2Int position, DirectionEnum direction, bool save = false)
    {
        SetPosition(position);
        Vector2 worldPos = MapManager.Instance.GetWorldPos(position);
        Quaternion rotation = Quaternion.Euler(Direction.GetDirection(direction));

        Building fabricInstnace = Instantiate(this, worldPos, rotation);
        fabricInstnace.Position = new BuildingSize(position, _buildingInfo.tileSize);
        MapManager.Instance.AddBuilding(fabricInstnace, save);
    }
}

[Serializable]
public class BuildingSize
{
    public Vector2Int center;
    public Vector2Int min, max;

    /// <summary>
    /// Vector2Int Position is overlap
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsOverlap(Vector2Int pos)
    {
        bool isOverlap = pos.x >= min.x && pos.x <= max.x && pos.y >= min.y && pos.y <= max.y;
        return isOverlap;
    }

    public bool IsOverlap(BuildingSize size)
    {
        Vector2Int[] edges = new Vector2Int[4]
        {
            size.min,
            new Vector2Int(size.min.x, size.max.y),
            new Vector2Int(size.max.x, size.min.y),
            size.max
        };

        for (int i = 0; i < 4; i++) 
            if (IsOverlap(edges[i])) return true;

        // �������� ��ġ�� �ʾ�����, ��ģ �κ��� ������ �� ����

        int left = size.min.x,
            right = size.max.x,
            top = size.max.y,
            bottom = size.min.y;

        //x�� �� �������� y�� �ּڰ��� �� �۰� �ִ��� �� Ŭ ���
        if (left >= min.x && left <= max.x && top >= max.y && bottom <= min.y)
        {
            return true;
        }
        if (right >= min.x && right <= max.x && top >= max.y && bottom <= min.y)
        {
            return true;
        }

        //�ݴ�� y�ุ ������ ���
        if (bottom >= min.y && bottom <= max.y && right > max.x && left < min.x)
        {
            return true;
        }
        if (top >= min.y && top <= max.y && right > max.x && left < min.x)
        {
            return true;
        }

        return false;
    }

    public BuildingSize(Vector2Int position, float size)
    {
        center = position;
        int halfSize = (int)size - 1;

        min = position;
        max = position + new Vector2Int(halfSize, halfSize);
    }
}