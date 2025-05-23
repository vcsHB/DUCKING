using UnityEngine;

namespace BuildingManage.Tower
{

    public class TowerTargetDetector : MonoBehaviour
    {
        [SerializeField] private float _detectRange = 7f;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private LayerMask _targetLayer;

        public bool CheckTarget(out Vector2 targetPos)
        {
            Collider2D target = Physics2D.OverlapCircle((Vector2)transform.position + _offset, _detectRange, _targetLayer);

            targetPos = Vector2.zero;
            if (target == null) return false;
            targetPos = target.transform.position;
            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)transform.position + _offset, _detectRange);
        }


    }

}