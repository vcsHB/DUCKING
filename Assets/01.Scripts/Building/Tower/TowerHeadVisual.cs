using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingManage.Tower
{
    public class TowerHeadVisual : MonoBehaviour
    {
        [SerializeField]
        private float _aimSpeed;
        private Quaternion _prevRotation;
        
        private Transform[] _towerHeadVisuals;

        private void Awake()
        {
            
            _towerHeadVisuals = new Transform[transform.childCount];
            
            
            int index = 0;
            foreach (Transform child in transform)
            {
                // if (index == 0)
                // {
                //     index++;// 처음껀 본인이라 제외
                //     continue;
                // }
                _towerHeadVisuals[index] = child;
                index++;
            }
        }

        private void FixedUpdate()
        {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            UpdateHeadDirection(direction.normalized);
        }

        internal void Initialize( float aimSpeed)
        {
            _aimSpeed = aimSpeed;
        }

        public void UpdateHeadDirection(Vector2 direction)
        {

            Quaternion rotation = Quaternion.Euler(0,0 ,Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);

            for (int i = 0; i < _towerHeadVisuals.Length; i++)
            {
                _towerHeadVisuals[i].rotation = Quaternion.Lerp(_towerHeadVisuals[i].rotation, rotation, Time.fixedDeltaTime * _aimSpeed);
            }
            //_prevRotation = rotation;
        }
        
    }

}