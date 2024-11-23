using System.Collections;
using BuildingManage;
using DG.Tweening;
using UnityEngine;
namespace Objects.UsableItem
{

    public class NeveroxBomb : UsableItem
    {
        [SerializeField] private ParticleSystem _explodeVFX;
        [SerializeField] private int _explodeRange = 4;
        [SerializeField] private float _explodeDelay = 5f;
        [SerializeField] private float _destroyDelay = 3f;
        [SerializeField] private Color _blinkColor = Color.red;
        [Header("Shake Setting")]
        [SerializeField] private int _shakeStrength = 1;
        [SerializeField] private int _shakeBivrato = 5;
        private Color _defaultColor;
        private SpriteRenderer _spriteRenderer;

        private void Awake() {
            
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _defaultColor = _spriteRenderer.color;
        }

        public override void Use(Vector2 position)
        {
            base.Use(position);
            StartCoroutine(ExplodeCoroutine());
        }

        private IEnumerator ExplodeCoroutine()
        {
            float currentTime = 0f;
            transform.DOShakePosition(_explodeDelay, _shakeStrength, _shakeBivrato);
            while(currentTime < _explodeDelay)
            {
                currentTime += Time.deltaTime; 
                _spriteRenderer.color = Color.Lerp(_defaultColor, _blinkColor, currentTime / _explodeDelay);
                yield return null;
            }
            _spriteRenderer.color = _blinkColor;
            Explode();
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(_destroyDelay);
            Destroy(gameObject);
        }

        private void Explode()
        {
            _explodeVFX.Play();
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