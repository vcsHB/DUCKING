using UnityEngine;

namespace BuildingManage
{
    public interface IBuildable
    {
        public abstract void Build(Vector2 position);
        public abstract void ReadyDestroy();
        public abstract void Destroy();
    }
}