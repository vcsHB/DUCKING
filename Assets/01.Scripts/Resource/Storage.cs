using UnityEngine;

namespace ResourceSystem
{
    [System.Serializable]
    public struct ResourceData
    {
        public int currentAmount;
        public int max;
    }
    public class Storage : MonoBehaviour, IResourceInput, IResourceOutput
    {
        [SerializeField] private SerializeDictionary<ResourceType, ResourceData> _resourceDictionary;
        
        public bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
        {
            if (_resourceDictionary.TryGetValue(resource.type, out ResourceData data))
            {
                if (data.currentAmount + resource.amount > data.max)
                {
                    remain.amount = data.currentAmount + resource.amount - data.max;
                    data.currentAmount = data.max;
                }
                data.currentAmount += resource.amount;

            }
            else
            {
                remain = resource;
                return false;
            }

            remain = new Resource();
            return true;
        }

        public void TransferResource()
        {
            
        }
    }
}