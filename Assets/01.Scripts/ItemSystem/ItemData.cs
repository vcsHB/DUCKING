using ResourceSystem;

namespace ItemSystem
{
    [System.Serializable]
    public class ItemData
    {
        public int id;
        public int amount;

        public ItemData() { }

        public ItemData(Resource resource)
        {
            id = (int)resource.type;
            amount = resource.amount;
        }
    }
}