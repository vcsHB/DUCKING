using System.Collections;
using ObjectPooling;
using UnityEngine;
using UnityEngine.Events;

public class VFXPlayer : MonoBehaviour, IPoolable
{
    [field:SerializeField] public PoolingType type { get; set; }
    public UnityEvent OnVFXPlayEvent;
    public GameObject ObjectPrefab => gameObject;
    private ParticleSystem _particleSystem;
    [SerializeField] private float _lifeTime;
    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public virtual void PlayVFX()
    {
        _particleSystem.Play();
        OnVFXPlayEvent?.Invoke();
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        PoolManager.Instance.Push(this);
    }

    public void ResetItem()
    {
        _particleSystem.Clear();
    }
    
}
