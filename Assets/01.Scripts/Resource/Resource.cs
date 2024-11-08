namespace ResourceSystem
{
    [System.Serializable]
    public struct Resource
    {
        public ResourceType type;
        public int amount;

        public Resource(ResourceType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }
}