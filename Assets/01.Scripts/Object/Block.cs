using System;
using System.Collections;
using System.Collections.Generic;
using AgentManage;
using UnityEngine;

namespace BlockManage
{
    
    public class Block : MonoBehaviour
    {
        public Health HealthCompo { get; protected set; }

        private void Awake()
        {
            HealthCompo = GetComponent<Health>();
        }
    }

}