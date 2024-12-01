using UnityEngine;

namespace BuildingManage
{
    public interface IBuildable
    {
        public abstract void Build(Vector2Int position, DirectionEnum direction, bool save = false);
        public abstract void Destroy();
    }
}