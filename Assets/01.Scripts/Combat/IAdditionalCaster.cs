using UnityEngine;

namespace Combat
{
    public interface IAdditionalCaster
    {
        public void Cast(Collider2D target);
    }
}