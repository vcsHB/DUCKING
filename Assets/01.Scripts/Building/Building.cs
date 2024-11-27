using AgentManage;
using BuildingManage;
using System;
using UnityEngine;
using ObjectPooling;

public abstract class Building : MonoBehaviour, IBuildable, ISelectable
{
    [SerializeField] protected BuildingSO _buildingInfo;
    public event Action<bool> OnSelectEvent;
    public event Action OnDestroyEvent;
    protected DirectionEnum direction;
    protected Transform _visualTrm;

    protected Health _healthCompo;
    public Health HealthCompo => _healthCompo;
   
    public BuildingSize Position { get; protected set; }
    public BuildingSO BuildingInfo => _buildingInfo;
    public BuildingEnum BuildingType => _buildingInfo.buildingType;
    public DirectionEnum BuildingDirection =>direction;

    protected virtual void Awake()
    {
        _healthCompo = GetComponent<Health>();
        _healthCompo.OnDieEvent.AddListener(Destroy);
        _healthCompo.SetMaxHealth(_buildingInfo.health);
        _healthCompo.ResetHealth();
        
        _visualTrm = transform.GetChild(0);
    }

    public bool CheckPosition(Vector2Int pos) => Position.IsOverlap(pos);
    public bool CheckPosition(BuildingSize pos) => Position.IsOverlap(pos);

    protected virtual void SetPosition(Vector2Int position)
    {
        Position = new BuildingSize(position, _buildingInfo.tileSize);
    }

    public virtual void SetRotation(DirectionEnum direction)
    {
        Quaternion rotation = Quaternion.Euler(Direction.GetDirection(direction));

        _visualTrm.rotation = rotation;
        this.direction = direction;

        MapManager.Instance.RotateBuilding(Position.min, direction);
    }

    public virtual void Destroy()
    {
        MapManager.Instance.RemoveBuilding(this, true);
        PoolingType destroyVFXType = PoolingType.BuildingDestroVFX_1;
        switch(_buildingInfo.tileSize)
        {
            case 2:
            destroyVFXType = PoolingType.BuildingDestroVFX_4;
            break;
        }
        VFXPlayer vfxPlayer = PoolManager.Instance.Pop(destroyVFXType, transform.position, Quaternion.identity) as VFXPlayer;
        vfxPlayer.PlayVFX();
        OnDestroyEvent?.Invoke();
        Destroy(gameObject);
    }

    public void ReadyDestroy()
    {

    }

    public virtual void Build(Vector2Int position, DirectionEnum direction, bool save = false)
    {
        Vector2 worldPos = MapManager.Instance.GetWorldPos(position);

        Building buildingInstance = Instantiate(this, worldPos, Quaternion.identity);
        buildingInstance.SetPosition(position);
        buildingInstance.SetRotation(direction);

        MapManager.Instance.AddBuilding(buildingInstance, save);
    }

    public Building GetInformation()
    {
        OnSelectEvent?.Invoke(true);
        return this;
    }

    public void UnSelect()
    {
        OnSelectEvent?.Invoke(false);
    }
}

[Serializable]
public class BuildingSize
{
    public Vector2 center;
    public Vector2Int min, max;
    public float size;

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

        // 꼭짓점이 겹치진 않았지만, 겹친 부분이 존재할 수 있음

        int left = size.min.x,
            right = size.max.x,
            top = size.max.y,
            bottom = size.min.y;

        //x축 만 겹쳤지만 y의 최솟값이 더 작고 최댓값이 더 클 경우
        if (left >= min.x && left <= max.x && top >= max.y && bottom <= min.y)
        {
            return true;
        }
        if (right >= min.x && right <= max.x && top >= max.y && bottom <= min.y)
        {
            return true;
        }

        //반대로 y축만 겹쳐진 경우
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

    public BuildingSize(Vector2Int position, int size)
    {
        int halfSize = size - 1;
        min = position;
        max = position + new Vector2Int(halfSize, halfSize);


        this.size = (float)size;
        Vector2 wMax = MapManager.Instance.GetWorldPos(max + Vector2Int.one);
        Vector2 wMin = MapManager.Instance.GetWorldPos(min);
        center = wMin + (wMax - wMin) / 2;
    }
}