using BuildingManage;
using ResourceSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transfortation : Building, IResourceInput, IResourceOutput
{
    public event Action OnTransferResource;

    protected List<DirectionEnum> _inputDirection = new List<DirectionEnum>();
    protected List<DirectionEnum> _outputDirection = new List<DirectionEnum>();

    protected virtual void OnEnable()
    {
        MapManager.Instance.BuildController.OnBuildingChange += UpdateInputOutput;
    }
    protected virtual void OnDisable()
    {
        if (!MapManager.IsDestroyed)
            MapManager.Instance.BuildController.OnBuildingChange -= UpdateInputOutput;
    }

    public override void Build(Vector2Int position, DirectionEnum direction, bool save = false)
    {
        base.Build(position, direction, save);
        CheckNeighbor(position);

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

    //public virtual void TransferResource()
    //{
    //    _outputDirection.ForEach(dir =>
    //    {
    //        //다음 위치를 가져오는 부분 1x1 사이즈 일때만 유효한 부분임
    //        Vector2Int nextPosition = Position.min + Direction.GetTileDirection(dir);

    //        //그 부분에 건물이 있고 그 건물이 IResourceInput을 가지고 있다면
    //        bool buildingExsist =
    //            MapManager.Instance.TryGetBuilding(nextPosition, out Building connectedBuilding);

    //        if (!buildingExsist || !connectedBuilding.TryGetComponent(out IResourceInput input)) return;

    //        //방향을 반대로 돌려서 input에 TryInsertResource를 호출해줘
    //        DirectionEnum opposite = Direction.GetOpposite(dir);

    //        for (int i = 0; i < _processes.Count; i++)
    //        {
    //            if (_processes[i] < 1) continue;

    //            input.TryInsertResource(_container, opposite, out Resource remain);
    //            if (remain.type == ResourceType.None)
    //            {
    //                _processes.RemoveAt(i);
    //                _container.RemoveAt(i);
    //                i--;
    //            }
    //        }
    //    });
    //}

    //public virtual bool TryInsertResource(Resource _container, DirectionEnum inputDir, out Resource remain)
    //{
    //    if (!_inputDirection.Contains(inputDir) || _container.)
    //    {
    //        remain = _container;
    //        return false;
    //    }

    //    remain = new Resource(ResourceType.None, 0);
    //    _container.Add(_container);
    //    _processes.Add(0);
    //    return true;
    //}

    protected virtual void UpdateInputOutput()
    {
        if (Position != null) CheckNeighbor(Position.min);
    }

    protected abstract void CheckNeighbor(Vector2Int position);

    public abstract bool TryInsertResource(Resource resource, DirectionEnum inputDir, out Resource remain);

    public abstract void TransferResource();


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
