using Objects.UsableItem;
using UnityEngine;
namespace ItemSystem
{

    [CreateAssetMenu(menuName = "SO/ItemSystem/UsableItemSO", order = 0)]
    public class UsableItemInfoSO : ItemInfoSO
    {
        public UsableItem itemPrefab;
        
    }
}