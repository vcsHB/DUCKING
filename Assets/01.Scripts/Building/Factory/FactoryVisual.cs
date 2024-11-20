using UnityEngine;

public class FactoryVisual : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Material _material;
    private int _factoryActiveHash = Shader.PropertyToID("_IsActive");

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;

    }

    public void HandleSetActive(bool isActive)
    {
        _material.SetInt(_factoryActiveHash, isActive? 1 : 0);

    }


}
