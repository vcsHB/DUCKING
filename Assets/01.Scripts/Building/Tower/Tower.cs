using Combat;
using ObjectPooling;
using ResourceSystem;
using UnityEngine;

namespace BuildingManage.Tower
{

    public class Tower : Building, IResourceInput
    {

        [Header("Essential Setting")]
        [SerializeField] protected TowerHeadVisual _headVisual;
        [SerializeField] protected TowerTargetDetector _targetDetector;
        [SerializeField] private ResourceType _needResource;

        [SerializeField] private Transform[] _gunTips;
        [SerializeField] private Vector2 _towerCenterOffset;

        [Header("Tower Setting")]
        [SerializeField] private float _fireCooltime;
        [SerializeField] protected float _aimingSpeed;

        [SerializeField] private PoolingType _projectile;
        [SerializeField] private PoolingType _fireVFX;
        private int _currentGunTipIndex;
        private float _currentCoolTime;
        private Vector2 _targetPos;

        private bool CanShoot => (_currentCoolTime > _fireCooltime);


        protected override void Awake()
        {
            base.Awake();
            _headVisual.Initialize(_aimingSpeed);
        }

        private void Update()
        {
            _currentCoolTime += Time.deltaTime;

        }

        private void FixedUpdate()
        {
            bool isCheck = _targetDetector.CheckTarget(out _targetPos);
            if (isCheck)
            {
                Vector2 direction = _targetPos - ((Vector2)transform.position + _towerCenterOffset);
                _headVisual.UpdateHeadDirection(direction.normalized);
                if (CanShoot)
                {
                    _currentCoolTime = 0;
                    Attack();
                }
            }
        }

        protected virtual void Attack()
        {
            Transform currentGunTip = _gunTips[_currentGunTipIndex];
            Vector2 direction = currentGunTip.up;

            Projectile bullet = PoolManager.Instance.Pop(_projectile, currentGunTip.position, Quaternion.identity) as Projectile;
            VFXPlayer vfx = PoolManager.Instance.Pop(_fireVFX, currentGunTip.position, Quaternion.identity) as VFXPlayer;
            bullet.Fire(direction);
            vfx.PlayVFX();

            _currentGunTipIndex = (_currentGunTipIndex + 1) % _gunTips.Length;
        }

        public bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
        {
            throw new System.NotImplementedException();
        }
    }

}