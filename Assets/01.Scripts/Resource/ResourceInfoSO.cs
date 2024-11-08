using UnityEngine;
using UnityEngine.Tilemaps;

namespace ResourceSystem
{
    [CreateAssetMenu(menuName = "SO/Resource/ResourceInfo")]
    public class ResourceInfoSO : ScriptableObject
    {
        public string resourceName;
        public string description;

        public TileBase resourceTile;
    }
}