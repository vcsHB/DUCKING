using System;
using System.Collections.Generic;
using System.Linq;
using StatSystem;
using UnityEngine;

namespace AgentManage
{
    
    public abstract class Agent : MonoBehaviour
    {
        public Health HealthCompo { get; private set; }
        [field:SerializeField] public StatusSO Stat { get; protected set; }
        protected Dictionary<Type, IAgentComponent> _components;

        protected virtual void Awake()
        {
            
            Stat = Instantiate(Stat);
            HealthCompo = GetComponent<Health>();
            HealthCompo.SetMaxHealth(Stat.health.GetValue());

            _components = new Dictionary<Type, IAgentComponent>();
            AddComponentToDictionary();
            ComponentInitialize();
            AfterInit();
        }

        private void AddComponentToDictionary()
        {
            GetComponentsInChildren<IAgentComponent>(true)
                .ToList().ForEach(compo => _components.Add(compo.GetType(), compo));
        }

        private void ComponentInitialize()
        {
            _components.Values.ToList().ForEach(compo => compo.Initialize(this));
        }

        private void AfterInit()
        {
            _components.Values.ToList().ForEach(compo => compo.AfterInit());
        }

        public T GetCompo<T>(bool isDerived = false) where T : class
        {
            if(_components.TryGetValue(typeof(T), out IAgentComponent compo))
            {
                return compo as T;
            }

            if (!isDerived) return default;

            Type findType = _components.Keys.FirstOrDefault(x => x.IsSubclassOf(typeof(T)));
            if (findType != null)
                return _components[findType] as T;

            return default(T);
        }

        private void OnDestroy()
        {
            // 현재로써는 파괴되는 순서에 문제가 있을 수 있음
            // 나중에 수업에서 다루며 수정
            _components.Values.ToList().ForEach(compo => compo.Dispose());
        }
       
    }

}