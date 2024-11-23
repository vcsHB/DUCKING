using ItemSystem;
using UnityEngine;
namespace AgentManage.Enemies
{

    public class EnemyItemDroper : MonoBehaviour, IAgentComponent
    {
        [SerializeField] private EnemyDropItemBaseSO _dropBase;
        private Enemy _enemy;
        public void Initialize(Agent agent)
        {
            _enemy = agent as Enemy;
        }
        public void AfterInit()
        {
            _enemy.HealthCompo.OnDieEvent.AddListener(HandleDropReward);
        }

        public void Dispose()
        {
             _enemy.HealthCompo.OnDieEvent.RemoveListener(HandleDropReward);
        }

        public void HandleDropReward()
        {
            for (int i = 0; i < _dropBase.drops.Length; i++)
            {
                DropBase dropBase = _dropBase.drops[i];
                if (dropBase.generateRate > Random.Range(0f, 1f))
                {
                    int dropAmount = Random.Range(dropBase.minAmount, dropBase.maxAmount);
                    ItemDropManager.Instance.GenerateDropItem(dropBase.id, dropAmount, transform.position);
                }
            }
        }

    }
}