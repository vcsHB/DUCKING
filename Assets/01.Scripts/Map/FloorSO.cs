using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "SO/FloorSO")]
public class FloorSO : ScriptableObject
{
    [Header("ForDebuging")]
    [Range(-10000, 10000)]
    [SerializeField] private float _seed;
    public float scale = 1;

    public List<FloorTileInfo> floorTiles = new List<FloorTileInfo>();

    #region Noise

    public float perlin(float x, float y) => Mathf.PerlinNoise((x + _seed) / scale, (y + _seed) / scale);

    public TileBase GetTile(float x, float y, float xSize, float ySize)
    {
        TileBase tile = null;
        float value = perlin(x / xSize, y / ySize);
        float ratioOffset = 0;

        int totalRatio = 0;
        floorTiles.ForEach(info => totalRatio += info.ratio);

        for(int i = 0; i < floorTiles.Count; i++)
        {
            FloorTileInfo info = floorTiles[i];

            float ratio = Mathf.Lerp(0, 1, (float)info.ratio / (float)totalRatio) + ratioOffset;
            ratioOffset = ratio;

            if (value <= ratio)
            {
                tile = info.tile;
                break;
            }
        }

        //floorTiles.ForEach(info =>
        //{
            
        //});

        return tile;
    }

    public void SetSeed(int seed)
    {
        _seed = seed;
    }


    #endregion
}

[Serializable]
public struct FloorTileInfo
{
    public TileBase tile;
    public int ratio;
}