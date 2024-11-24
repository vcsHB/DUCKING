using BuildingManage;

namespace ResourceSystem
{
    public interface IResourceInput
    {
        public bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain);
        
    }
}