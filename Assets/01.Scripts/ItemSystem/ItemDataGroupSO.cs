using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(menuName = "SO/ItemSystem/ItemDataGroup")]
    public class ItemDataGroupSO : ScriptableObject
    {
        public ItemSO[] itemList;

        public ItemSO GetItemData(int id)
        {
            for (int i = 0; i < itemList.Length; i++)
            {
                if (itemList[i].id == id)
                    return itemList[i];
            }
            Debug.LogError($"Not Exist Item Id : ID({id})");
            return null;
        }
    }
}