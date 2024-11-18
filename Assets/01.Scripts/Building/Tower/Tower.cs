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

        [SerializeField] private Transform[] _gunTips;
        [SerializeField] private Vector2 _towerCenterOffset;

        [Header("Tower Setting")]
        [SerializeField] private float _fireCooltime;
        [SerializeField] protected float _aimingSpeed;
        [SerializeField] private Resource _needResource;
        [SerializeField] protected int _maxResourceAmount;
        [SerializeField] protected int _currentResourceAmount; // 사실상 총알 수


        [SerializeField] private PoolingType _projectile;
        [SerializeField] private PoolingType _fireVFX;
        private int _currentGunTipIndex;
        private float _currentCoolTime;
        private Vector2 _targetPos;

        private bool CanShoot => (_currentCoolTime > _fireCooltime) && IsResourceEnough;
        private bool IsResourceEnough => _currentResourceAmount >= _needResource.amount;

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
                if (IsResourceEnough)
                {
                    Vector2 direction = _targetPos - ((Vector2)transform.position + _towerCenterOffset);
                    _headVisual.UpdateHeadDirection(direction.normalized);
                }
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
            _currentResourceAmount--;
            _currentGunTipIndex = (_currentGunTipIndex + 1) % _gunTips.Length;
        }

        public bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
        {
            remain = resource;
            if (_needResource.type != resource.type)
                return false;

            int sum = _currentResourceAmount + resource.amount;
            if (sum > _maxResourceAmount)
            {
                _currentResourceAmount = _maxResourceAmount;
                remain.amount = sum - _maxResourceAmount;
            }
            else
            {
                _currentResourceAmount = sum;
                remain.type = ResourceType.None;
            }

            return true;
        }
    }

}