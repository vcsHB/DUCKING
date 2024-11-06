using BuildingManage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private Transform _visualTrm;
    private Material _visualMat;

    [ColorUsage(true, true)]
    [SerializeField] private Color _succesColor, _failColor;

    private void Awake()
    {
        _visualMat = _visualTrm.GetComponent<SpriteRenderer>().material;
    }

    public void SetBuilding(int size)
    {
        _visualTrm.localScale = Vector2.one * size;
        _visualTrm.localPosition = Vector2.one * (size / 2.0f);

        _visualTrm.gameObject.SetActive(true);
    }

    public void Disable()
    {
        _visualTrm.gameObject.SetActive(false);
    }

    public void UpdateBuildidng(Vector2 position, int size)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position);
        Vector2Int tp = MapManager.Instance.GetTilePos(worldPosition);
        Vector2 tilePos = MapManager.Instance.RoundToTilePos(worldPosition);

        BuildingSize fabricSize = new BuildingSize(tp, size);
        bool isOverlap = MapManager.Instance.CheckBuildingOverlap(fabricSize);
        _visualMat.SetColor("_Color", isOverlap ? _failColor : _succesColor);

        transform.position = tilePos;
    }
}
