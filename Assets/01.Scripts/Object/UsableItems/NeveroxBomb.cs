using BuildingManage;
using UnityEngine;
namespace Objects.UsableItem
{

    public class NeveroxBomb : UseableItem
    {
        [SerializeField] private int _explodeRange = 4;


        [ContextMenu("TestBoom")]
        public void Explode()
        {
            int x = _intPosition.x;
            int y = _intPosition.y;
            MapManager.Instance.CorrosiumController.AddEncorrosive(new Vector2Int(x + _explodeRange, y + _explodeRange));
            MapManager.Instance.CorrosiumController.AddEncorrosive(new Vector2Int(x - _explodeRange, y + _explodeRange));
            MapManager.Instance.CorrosiumController.AddEncorrosive(new Vector2Int(x + _explodeRange, y - _explodeRange));
            MapManager.Instance.CorrosiumController.AddEncorrosive(new Vector2Int(x - _explodeRange, y - _explodeRange));
            MapManager.Instance.CorrosiumController.SetCorrosive();
        }
    }
}