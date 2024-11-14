using System;
using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;

public class Factory : Source, IResourceInput
{
    [Header("Factory Setting")]
    [SerializeField] protected SerializeDictionary<ResourceType, int> _requireResources;
    [SerializeField] protected Resource[] _outputResources;
    protected Dictionary<ResourceType, int> _storage;
    [SerializeField] protected int _storageSize;

    //public bool IsEnough => _requireResources.Keys.Where( type => 

    [SerializeField] protected float _processDuration = 2f;
    private float _currentTime = 0;

    /// <summary>
    /// 현재 제작 진행상황을 의미 (0~1f 값)
    /// </summary>
    public float CurrentProgress => _currentTime / _processDuration;

    public event Action<float> OnProgressEvent;
    public event Action OnProgressOverEvent;
    public bool IsProcessing { get; protected set; }


    protected override void Awake()
    {
        base.Awake();
        OnProgressOverEvent += HandleProgressOver;
    }

    private void OnDestroy()
    {
        OnProgressOverEvent -= HandleProgressOver;
    }


    private void Update()
    {

        if (!IsProcessing) return;
        _currentTime += Time.deltaTime;
        OnProgressEvent?.Invoke(CurrentProgress);
        if (CurrentProgress >= 1)
        {

            OnProgressOverEvent?.Invoke();
            OnProgressEvent?.Invoke(0);
        }
    }

    public bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
    {
        if (!_requireResources.ContainsKey(resource.type))
        {
            remain = resource;
            return false;
        }

        int plus = resource.amount + _storage[resource.type]; // 더해진 값
        remain.type = resource.type;
        if (plus >= _storageSize)
        {
            _storage[resource.type] = _storageSize;
            remain.amount = plus - _storageSize;
        }
        else
        {
            _storage[resource.type] = plus;
            remain.amount = 0;
        }

        HandleStartMakeProcess();
        return true;
    }

    private void HandleStartMakeProcess()
    {
        if (IsProcessing) return;
        IsProcessing = true;
        _currentTime = 0;

    }


    private void HandleProgressOver()
    {
        _currentTime = 0;
        IsProcessing = false;
        foreach (var item in _requireResources)
        {
            _storage[item.Key] -= item.Value; // 들어간 재료만큼 storage에서 차감
        }
        for (int i = 0; i < _outputResources.Length; i++)
        {
            _container.Add(_outputResources[i]);
        }
    }
}
