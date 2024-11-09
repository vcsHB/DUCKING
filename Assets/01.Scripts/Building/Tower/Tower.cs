using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingManage.Tower
{
    
    public class Tower : MonoBehaviour
    {

        [Header("Essential Setting")]
        [SerializeField] protected TowerHeadVisual _headVisual;
 
        [Header("Tower Setting")]
        [SerializeField] protected float _aimingSpeed;


        protected virtual void Awake()
        {
            _headVisual.Initialize(_aimingSpeed);
        }
    }

}