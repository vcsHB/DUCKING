using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPooling;
using UnityEngine;

public class VFXPlayer : MonoBehaviour, IPoolable
{
    [field:SerializeField] public PoolingType type { get; set; }
    public GameObject ObjectPrefab => gameObject;
    private ParticleSystem _particleSystem;
    [SerializeField] private float _lifeTime;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void PlayVFX()
    {
        _particleSystem.Play();
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
