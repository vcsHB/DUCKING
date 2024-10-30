using UnityEngine;

namespace BuildingManage
{
    public interface IBuildable
    {
        public abstract void Build(Vector2Int position, DirectionEnum direction);
        public abstract void ReadyDestroy();
        public abstract void Destroy();
    }
}