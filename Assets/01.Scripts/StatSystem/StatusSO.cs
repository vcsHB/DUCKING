using UnityEngine;

namespace StatSystem
{
    [CreateAssetMenu(menuName = "SO/Status")]
    public class StatusSO : ScriptableObject
    {
        public Stat health;
        public Stat moveSpeed;
        public Stat corrosionResist;
        
        
    }
}