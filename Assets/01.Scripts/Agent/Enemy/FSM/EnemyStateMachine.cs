
using System.Collections.Generic;
using UnityEngine;

namespace AgentManage.Enemys
{
    public class EnemyStateMachine
    {
        private Enemy _owner;
        public Dictionary<string, EnemyState> stateDictionary = new Dictionary<string, EnemyState>();
        public EnemyState CurrentState { get; private set; }
        public bool CanChangeState { get; private set; }
        private string firstState;

        public void Initialize(string firstStateName)
        {
            firstState = firstStateName;
            SetState(firstStateName);// 예외 처리 필요할지도
        }

        public void ResetState()
        {
            SetState(firstState);
        }

        private void SetState(string stateName)
        {
            CurrentState = stateDictionary[stateName]; // 예외 처리 필요할지도
            CurrentState.Enter();
        }

        public void AddState(string name, EnemyState state)
        {
            if (stateDictionary.ContainsKey(name))
            {
                Debug.Log("이럴순 없다");
                return;
            }
            stateDictionary.Add(name, state);

        }

        public void ChangeState(EnemyState newState, bool isForce = false)
        {
            if (!CanChangeState && !isForce) return;
            if (_owner.IsDead) return;

            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();

        }

        public EnemyState GetState(string stateName)
        {
            if (stateDictionary.TryGetValue(stateName, out EnemyState state))
            {
                return state;
            }
            Debug.Log("not Exist State");
            return null;
        }
    }
}