using UnityEngine;
namespace AgentManage.Enemies
{
    [System.Serializable]
    public struct DropBase 
    {
        public int id;
        public int minAmount, maxAmount;
        [Range(0f, 1f)]
        public float generateRate;
    }
    [CreateAssetMenu(menuName = "SO/Enemy/ItemDropBase")]
    public class EnemyDropItemBaseSO : ScriptableObject
    {
        public DropBase[] drops;
        
    }
}