using UnityEngine;

namespace Combat
{
    public class KnockBackCaster : MonoBehaviour, IAdditionalCaster
    {
        [SerializeField] private float _knockbackPower;
        [SerializeField] private float _knockbackDuration;
        
        public void Cast(Collider2D target)
        {
            Transform targetTrm = target.transform;
            Vector2 knockbackDirection = targetTrm.position - transform.position;

            if (target.TryGetComponent(out IKnockbackable hit))
            {
                hit.ApplyKnockback(knockbackDirection, _knockbackPower, _knockbackDuration);
            }
        }
    }
}