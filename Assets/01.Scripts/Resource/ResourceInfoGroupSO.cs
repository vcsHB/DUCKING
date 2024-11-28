using UnityEngine;
using UnityEngine.Tilemaps;

namespace ResourceSystem
{
    [CreateAssetMenu(menuName = "SO/ResourceGroup")]
    public class ResourceInfoGroupSO : ScriptableObject
    {
        public SerializeDictionary<ResourceType, ResourceInfoSO> resourceData;

        public ResourceInfoSO GetResourceInfo(ResourceType resourceType)
            => resourceData[resourceType];

        /// <summary>
        /// Get _container type by _container's name
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public ResourceType GetResourceType(string resourceName)
        {
            ResourceType resourceType = ResourceType.None;

            resourceData.SerializedKeys.ForEach(key =>
            {
                if(resourceData[key].resourceName == resourceName)
                {
                    resourceType = key;
                    return;
                }
            });

            return resourceType;
        }

        /// <summary>
        /// Get _container type by _container's tile
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public ResourceType GetResourceType(TileBase tile)
        {
            ResourceType resourceType = ResourceType.None;

            resourceData.SerializedKeys.ForEach(key =>
            {
                if (resourceData[key].resourceTile == tile)
                {
                    resourceType = key;
                    return;
                }
            });

            return resourceType;
        }
    }
}