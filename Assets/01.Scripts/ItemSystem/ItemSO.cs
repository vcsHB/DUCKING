using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "SO/ItemSystem/ItemSO")]
    public class ItemSO : ScriptableObject
    {
        /// <summary>
        /// 아이템 고유 ID 중복불가.
        /// </summary>
        public int id; // UNIQUE
        public string itemName;
        public string description;
        public Sprite itemSprite;
        /// <summary>
        /// 아이템 뭉쳐지는 개수
        /// </summary>
        public int itemMaxGroupingAmount;

    }
}