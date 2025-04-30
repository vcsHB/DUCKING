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
