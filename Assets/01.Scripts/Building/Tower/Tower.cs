using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using ObjectPooling;
using UnityEngine;

namespace BuildingManage.Tower
{
    
    public class Tower : MonoBehaviour
    {

        [Header("Essential Setting")]
        [SerializeField] protected TowerHeadVisual _headVisual;

        [SerializeField] private Transform[] _gunTips;

        [Header("Tower Setting")] 
        [SerializeField] private float _fireCooltime;
        [SerializeField] protected float _aimingSpeed;

        [SerializeField] private PoolingType _projectile;
        [SerializeField] private PoolingType _fireVFX;
        private int _currentGunTipIndex;
        private float _currentCoolTime;

        private bool CanShoot => (_currentCoolTime > _fireCooltime);

        protected virtual void Awake()
        {
            _headVisual.Initialize(_aimingSpeed);
        }

        private void Update()
        {
            _currentCoolTime += Time.deltaTime;
            
        }

        private void FixedUpdate()
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            _headVisual.UpdateHeadDirection(direction.normalized);
            if (Input.GetMouseButton(0) && CanShoot)
            {
                Attack();
                _currentCoolTime = 0;
            }
        }

        protected virtual void Attack()
        {
            Transform currentGunTip = _gunTips[_currentGunTipIndex];
            //Vector2 direction = currentGunTip.position - transform.position;
            Vector2 direction = currentGunTip.up;

            Projectile bullet = PoolManager.Instance.Pop(_projectile, currentGunTip.position, Quaternion.identity) as Projectile;
            VFXPlayer vfx = PoolManager.Instance.Pop(_fireVFX, currentGunTip.position, Quaternion.identity) as VFXPlayer;
            bullet.Fire(direction);
            vfx.PlayVFX();
            
            _currentGunTipIndex= (_currentGunTipIndex +  1) % _gunTips.Length;
        }
    }

}