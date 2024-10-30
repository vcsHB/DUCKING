namespace ResourceSystem
{
    public interface IResourceInput
    {
        public bool TryInsertResource(Resource resource, out Resource remain);
        
    }
}