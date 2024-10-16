using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MapManager : MonoSingleton<MapManager>
{
    [SerializeField] private Tilemap _tileMap;
    public Tilemap TileMap => _tileMap;

    private void Awake()
    {
        _tileMap.CompressBounds();
    }

    /// <summary>
    /// 월드 포지션을 받아서 타일맵의 포지션으로 바꿔줌
    /// </summary>
    /// <param name="pos">WorldPosition</param>
    /// <returns>Position in tilemap</returns>
    public Vector2Int GetTilePos(Vector2 pos) => (Vector2Int)_tileMap.WorldToCell(pos);

    /// <summary>
    /// 타일맵의 포지션을 받아서 월드 포지션으로 바꿔줌
    /// </summary>
    /// <param name="pos">Position in tilemap</param>
    /// <returns>WorldPosition</returns>
    public Vector2 GetWorldPos(Vector2Int pos) => (Vector2)_tileMap.CellToWorld((Vector3Int)pos);

    /// <summary>
    /// 벡터값을 받아서 타일맵에 딱들어맞는 포지션으로 바꿔줌
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2 RoundToTilePos(Vector2 pos) => (Vector2)_tileMap.CellToWorld(_tileMap.WorldToCell(pos));
}
