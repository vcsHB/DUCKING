using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingManage
{
    [CreateAssetMenu(menuName = "SO/Logic")]
    public class ConveyorLogicSO : ScriptableObject
    {
        public ResourceType applyResource;

        public bool CheckLogic(ResourceType resource) 
            => resource == applyResource;
    }
}
