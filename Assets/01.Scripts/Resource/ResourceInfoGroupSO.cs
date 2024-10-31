using UnityEngine;

namespace ResourceSystem
{
    [CreateAssetMenu(menuName = "SO/ResourceGroup")]
    public class ResourceInfoGroupSO : ScriptableObject
    {
        public SerializeDictionary<ResourceType, ResourceInfoSO> resourceData;
    }
}