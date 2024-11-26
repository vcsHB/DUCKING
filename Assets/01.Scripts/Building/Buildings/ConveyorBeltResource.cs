using ItemSystem;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConveyorBeltResource : MonoBehaviour
{
    [SerializeField] private ResourceInfoGroupSO _resourceGroup;
    private Resource _resource;
    private Vector2 _from, _to, _center;
    private SpriteRenderer _visual;
    private float _speed, _process = 0;
    private bool _isLast;

    private void Awake()
    {
        _visual = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (_process >= 1)
        {
            if (_isLast) ItemDropManager.Instance.GenerateDropItem((int)_resource.type, _resource.amount, _to);
            Destroy(gameObject);
        }

        _process += Time.deltaTime * _speed;
        Move(_process);
    }

    /// <summary>
    /// Process of Resource's Move
    /// </summary>
    /// <param name="process"> 0 ~ 1 Value</param>
    public void Move(float process)
    {
        if (process < 0.5f)
        {
            transform.position = Vector2.Lerp(_from, _center, process * 2);
        }
        else
        {
            transform.position = Vector2.Lerp(_center, _to, (process - 0.5f) * 2);
        }
    }

    public void DisableResource() => gameObject.SetActive(false);

    public void Init(Vector2 center, Vector2 from, Vector2 to, Resource resource, float speed, bool lastBelt)
    {
        if (resource.type == ResourceType.None) return;

        //어떤 리소스인지 스프라이트 넣어주기
        _resource = resource;
        _visual.sprite = _resourceGroup.GetResourceInfo(resource.type).icon;
        transform.position = from;

        _center = center;
        _from = from;
        _to = to;
        _speed = speed;
        _isLast = lastBelt;
    }
}
