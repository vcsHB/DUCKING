using BuildingManage;
using ItemSystem;
using ResourceSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

public class Transfortation : Building, IResourceInput, IResourceOutput
{
    public event Action OnTransferResource;
    protected Resource _container = new Resource(ResourceType.None, 0);

    protected List<DirectionEnum> _inputDirection = new List<DirectionEnum>();
    protected List<DirectionEnum> _outputDirection = new List<DirectionEnum>();

    protected virtual void OnEnable()
    {
        MapManager.Instance.BuildController.OnBuildingChange += UpdateInputs;
    }
    protected virtual void OnDisable()
    {
        if (!MapManager.IsDestroyed)
            MapManager.Instance.BuildController.OnBuildingChange -= UpdateInputs;
    }

    public override void Build(Vector2Int position, DirectionEnum direction, bool save = false)
    {
        base.Build(position, direction, save);
        MapManager.Instance.TryGetBuilding(position, out Building buildingInstance);
        Transfortation transfortation = (buildingInstance as Transfortation);
        transfortation.CheckNeighbor(position);

        MapManager.Instance.RotateBuilding(Position.min, direction);
    }

    public override void Destroy()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2Int connected = Position.min + Direction.directionsInt[i];
            MapManager.Instance.TryGetBuilding(connected, out Building building);

            if (building != null && building is Transfortation trans)
            {
                trans.RemoveInputDirection((DirectionEnum)((i + 2) % 4));
            }
        }

        base.Destroy();
    }

    public virtual void TransferResource()
    {
        _outputDirection.ForEach(dir =>
        {
            //���� ��ġ�� �������� �κ� 1x1 ������ �϶��� ��ȿ�� �κ���
            Vector2Int nextPosition = Position.min + Direction.GetTileDirection(dir);

            //�� �κп� �ǹ��� �ְ� �� �ǹ��� IResourceInput�� ������ �ִٸ�
            bool buildingExsist =
                MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);

            if (!buildingExsist)
            {
                OnGenerateDropItem();
                return;
            }

            if (!connectedBuilding.TryGetComponent(out IResourceInput input)) return;

            //������ �ݴ�� ������ input�� TryInsertResource�� ȣ������
            DirectionEnum opposite = Direction.GetOpposite(dir);
            input.TryInsertResource(_container, opposite, out _container);

            if (_container.type == ResourceType.None) return;
        });
    }

    public virtual bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain)
    {
        if (!_inputDirection.Contains(inputDir) || _container.type != ResourceType.None)
        {
            remain = resource;
            return false;
        }

        remain = new Resource(ResourceType.None, 0);
        _container = resource;
        return true;
    }

    protected virtual void OnGenerateDropItem()
    {

    }

    protected virtual void UpdateInputs()
    {
        if (Position != null) CheckNeighbor(Position.min);
    }

    protected virtual void CheckNeighbor(Vector2Int position)
    {
        
    }

    #region Directions

    public virtual void SetInputDirection(DirectionEnum inputDir)
    {
        if (!_inputDirection.Contains(inputDir))
            _inputDirection.Add(inputDir);
    }

    public virtual void SetOutputDirection(DirectionEnum outputDir)
    {
        if (!_outputDirection.Contains(outputDir))
            _outputDirection.Add(outputDir);
    }

    public virtual void RemoveInputDirection(DirectionEnum directionEnum)
    {
        _inputDirection.Remove(directionEnum);
    }

    public void RemoveOutputDirection(DirectionEnum directionEnum)
    {
        _outputDirection.Remove(directionEnum);
    }

    public bool ContainOutput(DirectionEnum directionEnum)
    {
        return _outputDirection.Contains(directionEnum);
    }

    public bool ContainInput(DirectionEnum directionEnum)
    {
        return _inputDirection.Contains(directionEnum);
    }

    #endregion
}
