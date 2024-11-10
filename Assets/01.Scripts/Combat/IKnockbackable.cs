using UnityEngine;

namespace Combat
{
    public interface IKnockbackable
    {
        public void ApplyKnockback(Vector2 direction, float power, float duration);
    }
}