using BuildingManage;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CorrosiumController : MonoBehaviour
{
    [SerializeField] private EncorrosiumSO _corrosiumSO;
    [SerializeField] private MapInfoSO _mapInfo;
    [SerializeField] private Tilemap _corrosiveTilemap;
    private System.Random _random;

    private bool[,] _isCorrosive;

    public List<Vector2Int> EncorrosiveAreaEdges { get; private set; }

    //private void Awake()
    //{
    //    SetRandomEncorrosiveArea(363482);
    //}

    public void SetRandomEncorrosiveArea(int seed)
    {
        _random = new System.Random(seed);

        Vector2Int size = _mapInfo.mapSize;
        _isCorrosive = new bool[size.x + 1, size.y + 1];

        EncorrosiveAreaEdges = new List<Vector2Int>();

        int hSize = _random.Next(_corrosiumSO.minSize, _corrosiumSO.maxSize);
        int vSize = _random.Next(_corrosiumSO.minSize, _corrosiumSO.maxSize);

        int left = _random.Next(0, hSize), right = hSize - left;
        int bottom = _random.Next(0, vSize), top = vSize - bottom;

        AddEncorrosive(new Vector2Int(-left, top));
        AddEncorrosive(new Vector2Int(-left, -bottom));
        AddEncorrosive(new Vector2Int(right, top));
        AddEncorrosive(new Vector2Int(right, -bottom));

        _corrosiumSO.shapes.ForEach(shape =>
        {
            int cnt = _random.Next(shape.minCount, shape.maxCount);

            while (--cnt > 0)
            {
                Vector2Int center = Vector2Int.zero;
                int number = _random.Next(0, 5);
                int sizeX = _random.Next(shape.minSize, shape.maxSize);
                int sizeY = _random.Next(shape.minSize, shape.maxSize);

                switch (number)
                {
                    case 0:
                        center = new Vector2Int(-left, _random.Next(-bottom, top + 1));

                        AddEncorrosive(center + new Vector2Int(0, sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(0, -sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(-sizeX, sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(-sizeX, -sizeY / 2));
                        break;
                    case 1:
                        center = new Vector2Int(right, _random.Next(-bottom, top + 1));

                        AddEncorrosive(center + new Vector2Int(0, sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(0, -sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(sizeX, sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(sizeX, -sizeY / 2));
                        break;
                    case 2:
                        center = new Vector2Int(_random.Next(-left, right + 1), -bottom);

                        AddEncorrosive(center + new Vector2Int(sizeX / 2, 0));
                        AddEncorrosive(center + new Vector2Int(-sizeX / 2, 0));
                        AddEncorrosive(center + new Vector2Int(sizeX / 2, -sizeY));
                        AddEncorrosive(center + new Vector2Int(-sizeX / 2, -sizeY));
                        break;
                    case 3:
                        center = new Vector2Int(_random.Next(-left, right + 1), top);

                        AddEncorrosive(center + new Vector2Int(sizeX / 2, 0));
                        AddEncorrosive(center + new Vector2Int(-sizeX / 2, 0));
                        AddEncorrosive(center + new Vector2Int(sizeX / 2, sizeY));
                        AddEncorrosive(center + new Vector2Int(-sizeX / 2, sizeY));
                        break;
                }
            }
        });

        SetCorrosive();
    }

    public void AddEncorrosive(Vector2Int edge)
    {
        if (EncorrosiveAreaEdges.Contains(edge)) return;
        EncorrosiveAreaEdges.Add(edge);
    }

    public void SetCorrosive()
    {
        Vector2Int size = _mapInfo.mapSize;

        for (int i = 0; i <= size.x; i++)
        {
            for (int j = 0; j <= size.y; j++)
            {
                _isCorrosive[i, j] = true;
            }
        }

        EncorrosiveAreaEdges.ForEach(e => SetDiagonal(e));

        for (int i = 0; i <= size.x; i++)
        {
            for (int j = 0; j <= size.y; j++)
            {
                int x = i - (size.x / 2);
                int y = j - (size.y / 2);

                if (_isCorrosive[i, j])
                {
                    _corrosiveTilemap.SetTile
                        (new Vector3Int(x, y), _corrosiumSO.corrosiumTile);
                }
            }
        }
    }

    private void SetDiagonal(Vector2Int edge)
    {
        int minX = int.MinValue;
        int minY = int.MinValue;

        EncorrosiveAreaEdges.Where(e => edge != e && e.x == edge.x && e.y > edge.y)
            .ToList().ForEach(e => minY = Mathf.Max(minY, e.y));
        EncorrosiveAreaEdges.Where(e => edge != e && e.y == edge.y && e.x > edge.x)
            .ToList().ForEach(e => minX = Mathf.Max(minX, e.x));

        if (minX == int.MinValue || minY == int.MinValue) return;

        for (int i = edge.x; i <= minX; i++)
        {
            for (int j = edge.y; j <= minY; j++)
            {
                int x = i + (_mapInfo.mapSize.x / 2);
                int y = j + (_mapInfo.mapSize.y / 2);

                _isCorrosive[x, y] = false;
            }
        }
    }
}