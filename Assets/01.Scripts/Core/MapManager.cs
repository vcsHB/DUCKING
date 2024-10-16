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
    /// ���� �������� �޾Ƽ� Ÿ�ϸ��� ���������� �ٲ���
    /// </summary>
    /// <param name="pos">WorldPosition</param>
    /// <returns>Position in tilemap</returns>
    public Vector2Int GetTilePos(Vector2 pos) => (Vector2Int)_tileMap.WorldToCell(pos);

    /// <summary>
    /// Ÿ�ϸ��� �������� �޾Ƽ� ���� ���������� �ٲ���
    /// </summary>
    /// <param name="pos">Position in tilemap</param>
    /// <returns>WorldPosition</returns>
    public Vector2 GetWorldPos(Vector2Int pos) => (Vector2)_tileMap.CellToWorld((Vector3Int)pos);

    /// <summary>
    /// ���Ͱ��� �޾Ƽ� Ÿ�ϸʿ� �����´� ���������� �ٲ���
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2 RoundToTilePos(Vector2 pos) => (Vector2)_tileMap.CellToWorld(_tileMap.WorldToCell(pos));
}
