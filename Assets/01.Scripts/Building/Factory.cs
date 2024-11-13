using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float _currentTime= 0;

    /// <summary>
    /// 현재 제작 진행상황을 의미 (0~1f 값)
    /// </summary>
    public float CurrentProgress => _currentTime / _processDuration;

    public bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
    {
        if(!_requireResources.ContainsKey(resource.type))
        {
            remain = resource;
            return false;
        }

        int over = resource.amount;
        remain = resource;

        return true;
    }
}
