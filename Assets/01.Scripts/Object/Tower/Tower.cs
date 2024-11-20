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
        [SerializeField] protected int _currentResourceAmount; // 자원 수
        [SerializeField] protected int _bulletMultiple = 1; // 1자원당 탄환 환원비율
        protected int _currentBullet = 0; // 실질적인 현재 탄수


        [SerializeField] private PoolingType _projectile;
        [SerializeField] private PoolingType _fireVFX;
        private int _currentGunTipIndex;
        private float _currentCoolTime;
        private Vector2 _targetPos;

        private bool CanShoot => (_currentCoolTime > _fireCooltime) && IsBulletEnough;
        private bool IsBulletEnough => _currentBullet > 0;
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
                Vector2 direction = _targetPos - ((Vector2)transform.position + _towerCenterOffset);
                _headVisual.UpdateHeadDirection(direction.normalized);
                RefillBullets();
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
            _currentBullet--;

            _currentGunTipIndex = (_currentGunTipIndex + 1) % _gunTips.Length;
        }

        private void RefillBullets()
        {
            if (_currentBullet <= 0) // 탄 부족시
            {
                if (_currentResourceAmount > 0)
                {
                    _currentResourceAmount--; // 자원 땡겨서 장전
                    _currentBullet = _bulletMultiple;
                }
            }
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