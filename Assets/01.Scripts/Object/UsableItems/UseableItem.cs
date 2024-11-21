using System;
using UnityEngine;
namespace Objects.UsableItem
{
    public class UseableItem : MonoBehaviour
    {
        [SerializeField] protected Vector2Int _intPosition;
        public event Action OnUseEvent;

        public virtual void Use(Vector2 position)
        {
            _intPosition = new Vector2Int((int)position.x, (int)position.y);
            transform.position = (Vector2)_intPosition;
            OnUseEvent?.Invoke();
        }
    }
}