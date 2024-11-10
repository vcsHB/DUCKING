using System.Collections;
using System.Collections.Generic;
using ObjectPooling;
using UnityEngine;

namespace AgentManage.Enemys
{

    public class Enemy : Agent, IPoolable
    {
        [field:SerializeField] public PoolingType type { get; set; }

        public GameObject ObjectPrefab => gameObject;

        public void ResetItem()
        {
            throw new System.NotImplementedException();
        }

        
    }

}