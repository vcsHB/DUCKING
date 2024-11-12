using UnityEngine;

namespace Combat
{
    public class EffectStateCaster : MonoBehaviour, IAdditionalCaster
    {
        public void Cast(Collider2D target)
        {
            // 나중에 버프 이펙트 시스템 개발되면 추가.
        }
    }
}