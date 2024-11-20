using System;
using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;

public class Factory : Source, IResourceInput, IOverloadable
{
    [Header("Factory Setting")]
    [SerializeField] protected SerializeDictionary<ResourceType, int> _requireResources;
    [SerializeField] protected Resource[] _outputResources;
    protected SerializeDictionary<ResourceType, int> _storage;
    [SerializeField] protected int _storageSize;
    [SerializeField] protected float _processDuration = 2f;
    [SerializeField] private FactoryVisual _factoryVisual;
    private float _currentTime = 0;


    /// <summary>
    /// 현재 제작 진행상황을 의미 (0~1f 값)
    /// </summary>
    public float CurrentProgress => _currentTime / _processDuration;

    public event Action<float> OnProgressEvent;
    public event Action OnProgressOverEvent;
    public bool IsProcessing { get; protected set; }
    float IOverloadable.OverloadLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    protected override void Awake()
    {
        base.Awake();
        OnProgressOverEvent += HandleProgressOver;
        _storage = new SerializeDictionary<ResourceType, int>();
        foreach (ResourceType type in _requireResources.Keys)
        {
            _storage.Add(type, 0);
        }
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

        if(_container.Count > 0)
            TransferResource();
    }

    public bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
    {
        print($"아이템 넣기 시도 {resource.type.ToString()}, {resource.amount.ToString()}");

        if (!_requireResources.ContainsKey(resource.type))
        {
            remain = resource;
            print("밍");
            return false;
        }

        int plus = resource.amount + _storage[resource.type]; // 더해진 값
        remain.type = resource.type;
        if (plus >= _storageSize)
        {
            _storage[resource.type] = _storageSize;
            remain.amount = plus - _storageSize;
            TryStartProcess();
            return false;
        }
        else
        {
            _storage[resource.type] = plus;
            resource.type = ResourceType.None;
            remain.amount = 0;
        }
        remain = new Resource();
        remain.type = ResourceType.None;
        TryStartProcess();
        return true;
    }

    private void TryStartProcess()
    {
        if (IsProcessing) return; // 이미 재작중이면 예외

        foreach (var require in _requireResources) // 충분한지 체크
        {
            if (_storage[require.Key] < require.Value) //  저장량이 요구량보다 적으면
            {
                // 걍 안돌리고 나감
                return;
            }
        }
        // 여기까지 왔다? 그럼 재료는 충분한 것인
        HandleStartMakeProcess();
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
