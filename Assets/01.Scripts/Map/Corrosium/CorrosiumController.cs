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
    private System.Random _random;

    [SerializeField] private EncorrosiumSO _corrosiumSO;
    [SerializeField] private MapInfoSO _mapInfo;
    [SerializeField] private Tilemap _corrosiveTilemap;
    [SerializeField] private Transform _centerTrm;

    public List<CorrosiumRectangle> EncorrosiveAreaEdges { get; private set; }

    public void SetRandomEncorrosiveArea(int seed)
    {
        EncorrosiveAreaEdges = new List<CorrosiumRectangle>();
        _path = Path.Combine(Application.dataPath, "Saves/Corrosive.json");

        if (File.Exists(_path))
        {
            Load();
            return;
        }

        _random = new System.Random(seed);

        int horizontalSize = _random.Next(_corrosiumSO.minSize, _corrosiumSO.maxSize);
        int verticalSize = _random.Next(_corrosiumSO.minSize, _corrosiumSO.maxSize);

        int left = _random.Next(0, horizontalSize), right = horizontalSize - left;
        int bottom = _random.Next(0, verticalSize), top = verticalSize - bottom;

        Vector2Int goalCenter = new Vector2Int((int)_centerTrm.position.x, (int)_centerTrm.position.y);
        Vector2Int min = goalCenter + new Vector2Int(-left, -bottom);
        Vector2Int max = goalCenter + new Vector2Int(right, top);
        AddEncorrosive(min, max);

        _corrosiumSO.shapes.ForEach(shape =>
        {
            int cnt = _random.Next(shape.minCount, shape.maxCount);

            while (--cnt > 0)
            {
                Vector2Int center = new Vector2Int((int)_centerTrm.position.x, (int)_centerTrm.position.y);
                int number = _random.Next(0, 5);
                int sizeX = _random.Next(shape.minSize, shape.maxSize);
                int sizeY = _random.Next(shape.minSize, shape.maxSize);
                Vector2Int min, max;

                switch (number)
                {
                    case 0:
                        center += new Vector2Int(-left, _random.Next(-bottom, top + 1));

                        min = center + new Vector2Int(-sizeX, -sizeY / 2);
                        max = center + new Vector2Int(0, sizeY / 2);
                        AddEncorrosive(min, max);

                        break;
                    case 1:
                        center += new Vector2Int(right, _random.Next(-bottom, top + 1));

                        min = center + new Vector2Int(0, -sizeY / 2);
                        max = center + new Vector2Int(sizeX, sizeY / 2);
                        AddEncorrosive(min, max);

                        break;
                    case 2:
                        center = center + new Vector2Int(_random.Next(-left, right + 1), -bottom);

                        min = center + new Vector2Int(-sizeX / 2, -sizeY);
                        max = center + new Vector2Int(sizeX / 2, 0);
                        AddEncorrosive(min, max);

                        break;
                    case 3:
                        center += new Vector2Int(_random.Next(-left, right + 1), top);

                        min = center + new Vector2Int(-sizeX / 2, 0);
                        max = center + new Vector2Int(sizeX / 2, sizeY);
                        AddEncorrosive(min, max);

                        break;
                }
            }
        });

        UpdateTile();
        Save();
    }

    public void AddEncorrosive(Vector2Int min, Vector2Int max)
    {
        if (min.x == max.x || min.y == max.y) return;

        EncorrosiveAreaEdges.Add(new CorrosiumRectangle(min, max));
        Save();
    }

    public void UpdateTile()
    {
        Vector2Int size = _mapInfo.mapSize;

        for (int i = 0; i <= size.x; i++)
        {
            for (int j = 0; j <= size.y; j++)
            {
                int x = i - (size.x / 2);
                int y = j - (size.y / 2);
                _corrosiveTilemap.SetTile(new Vector3Int(x, y), _corrosiumSO.corrosiumTile);
            }
        }

        EncorrosiveAreaEdges.ForEach(rect =>
        {
            for (int i = rect.min.x; i < rect.max.x; i++)
            {
                for (int j = rect.min.y; j < rect.max.y; j++)
                {
                    _corrosiveTilemap.SetTile(new Vector3Int(i, j), null);
                }
            }
        });

    }

    //Move All CorrosiumArea
    public void MoveCorrosive(Vector2Int direction)
    {
        EncorrosiveAreaEdges.ForEach(rect =>
        {
            rect.min += direction;
            rect.max += direction;
        });
    }


    #region Save&Load

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
        EncorrosiveAreaEdges = save.edges;

        UpdateTile();
    }

    public void ResetSaveData()
    {
        if (File.Exists(_path)) File.Delete(_path);
    }

    #endregion
}

[Serializable]
public class CorrosiumSave
{
    public List<CorrosiumRectangle> edges = new();
}

[Serializable]
public struct CorrosiumRectangle
{
    public Vector2Int min;
    public Vector2Int max;

    public CorrosiumRectangle(Vector2Int min, Vector2Int max)
    {
        this.min = min;
        this.max = max;
    }

    public bool IsOverlap(CorrosiumRectangle rect)
    {
        if (min.x < rect.min.x && max.x > rect.min.x && min.y < rect.min.y && max.y > rect.min.y) return true;
        if (min.x < rect.min.x && max.x > rect.min.x && min.y < rect.max.y && max.y > rect.max.y) return true;
        if (min.x < rect.max.x && max.x > rect.max.x && min.y < rect.min.y && max.y > rect.min.y) return true;
        if (min.x < rect.max.x && max.x > rect.max.x && min.y < rect.max.y && max.y > rect.max.y) return true;

        return false;
    }
}
