using BuildingManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CorrosiumController : MonoBehaviour
{
    private string _path;

    [SerializeField] private EncorrosiumSO _corrosiumSO;
    [SerializeField] private MapInfoSO _mapInfo;
    [SerializeField] private Tilemap _corrosiveTilemap;
    [SerializeField] private Transform _centerTrm;
    private System.Random _random;

    private bool[,] _isCorrosive;

    public List<Vector2Int> EncorrosiveAreaEdges { get; private set; }

    public void SetRandomEncorrosiveArea(int seed)
    {
        Vector2Int size = _mapInfo.mapSize;
        _isCorrosive = new bool[size.x + 1, size.y + 1];
        EncorrosiveAreaEdges = new List<Vector2Int>();

        _path = Path.Combine(Application.dataPath, "Saves/Corrosive.json");

        if (File.Exists(_path))
        {
            Load();
            return;
        }
        Debug.Log("นึ...");
        _random = new System.Random(seed);

        int hSize = _random.Next(_corrosiumSO.minSize, _corrosiumSO.maxSize);
        int vSize = _random.Next(_corrosiumSO.minSize, _corrosiumSO.maxSize);

        int left = _random.Next(0, hSize), right = hSize - left;
        int bottom = _random.Next(0, vSize), top = vSize - bottom;

        Vector2Int goalCenter = new Vector2Int((int)_centerTrm.position.x, (int)_centerTrm.position.y);
        AddEncorrosive(goalCenter + new Vector2Int(-left, top));
        AddEncorrosive(goalCenter + new Vector2Int(-left, -bottom));
        AddEncorrosive(goalCenter + new Vector2Int(right, top));
        AddEncorrosive(goalCenter + new Vector2Int(right, -bottom));

        _corrosiumSO.shapes.ForEach(shape =>
        {
            int cnt = _random.Next(shape.minCount, shape.maxCount);

            while (--cnt > 0)
            {
                Vector2Int center = new Vector2Int((int)_centerTrm.position.x, (int)_centerTrm.position.y);
                int number = _random.Next(0, 5);
                int sizeX = _random.Next(shape.minSize, shape.maxSize);
                int sizeY = _random.Next(shape.minSize, shape.maxSize);

                switch (number)
                {
                    case 0:
                        center = center + new Vector2Int(-left, _random.Next(-bottom, top + 1));

                        AddEncorrosive(center + new Vector2Int(0, sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(0, -sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(-sizeX, sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(-sizeX, -sizeY / 2));
                        break;
                    case 1:
                        center = center + new Vector2Int(right, _random.Next(-bottom, top + 1));

                        AddEncorrosive(center + new Vector2Int(0, sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(0, -sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(sizeX, sizeY / 2));
                        AddEncorrosive(center + new Vector2Int(sizeX, -sizeY / 2));
                        break;
                    case 2:
                        center = center + new Vector2Int(_random.Next(-left, right + 1), -bottom);

                        AddEncorrosive(center + new Vector2Int(sizeX / 2, 0));
                        AddEncorrosive(center + new Vector2Int(-sizeX / 2, 0));
                        AddEncorrosive(center + new Vector2Int(sizeX / 2, -sizeY));
                        AddEncorrosive(center + new Vector2Int(-sizeX / 2, -sizeY));
                        break;
                    case 3:
                        center = center + new Vector2Int(_random.Next(-left, right + 1), top);

                        AddEncorrosive(center + new Vector2Int(sizeX / 2, 0));
                        AddEncorrosive(center + new Vector2Int(-sizeX / 2, 0));
                        AddEncorrosive(center + new Vector2Int(sizeX / 2, sizeY));
                        AddEncorrosive(center + new Vector2Int(-sizeX / 2, sizeY));
                        break;
                }
            }
        });

        SetCorrosive();
        Save();
    }

    public void AddEncorrosive(Vector2Int edge)
    {
        if (EncorrosiveAreaEdges.Contains(edge)) return;
        EncorrosiveAreaEdges.Add(edge);
        Save();
    }

    public void AddEncorrosive(Vector2Int min, Vector2Int max)
    {
        Vector2Int size = _mapInfo.mapSize;
        Vector2Int leftUp = new Vector2Int(min.x, max.y);
        Vector2Int rightDown = new Vector2Int(max.x, min.y);

        if (!EncorrosiveAreaEdges.Contains(min)) EncorrosiveAreaEdges.Add(min);
        if (!EncorrosiveAreaEdges.Contains(max)) EncorrosiveAreaEdges.Add(max);
        if (!EncorrosiveAreaEdges.Contains(leftUp)) EncorrosiveAreaEdges.Add(leftUp);
        if (!EncorrosiveAreaEdges.Contains(rightDown)) EncorrosiveAreaEdges.Add(rightDown);

        Vector2Int position;
        for (int i = min.x; i < max.x; i++)
        {
            for (int j = min.y; j < max.y; j++)
            {
                position = new Vector2Int(i + size.x / 2, j + size.y / 2);
                _isCorrosive[position.x, position.y] = true;

                _corrosiveTilemap.SetTile(new Vector3Int(i, j), null);
            }
        }

        Save();
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
                else
                {
                    _corrosiveTilemap.SetTile(new Vector3Int(x, y), null);
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

    public void Save()
    {
        CorrosiumSave save = new CorrosiumSave();
        save.edges = EncorrosiveAreaEdges;

        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(_path, json);
    }

    public void Load()
    {
        string json = File.ReadAllText(_path);
        CorrosiumSave save = JsonUtility.FromJson<CorrosiumSave>(json);

        EncorrosiveAreaEdges = new List<Vector2Int>();
        save.edges.ForEach(e => EncorrosiveAreaEdges.Add(e));
        SetCorrosive();
    }

    public void ResetSaveData()
    {
        if(File.Exists(_path))
            File.Delete(_path);
    }
}

[Serializable]
public class CorrosiumSave
{
    public List<Vector2Int> edges = new();
}
