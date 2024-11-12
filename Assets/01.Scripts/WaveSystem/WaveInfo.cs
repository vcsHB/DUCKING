using ObjectPooling;
namespace WaveSystem
{
    [System.Serializable]
    public struct WaveInfo
    {
        public PoolingType enemyType;
        public int amount;
        public float generateTerm;
    }
}