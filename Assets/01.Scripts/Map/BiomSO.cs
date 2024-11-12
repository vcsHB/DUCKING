using ResourceSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "SO/FloorSO")]
public class BiomSO : ScriptableObject
{
    [Header("ForDebuging")]
    [Range(-10000, 10000)]
    public float seed;
    public float scale = 1;

    [Header("얼마나 광맥이 밀집해있는지 높을 수록 밀집도가 낮아지는")]
    public int chunkSize = 10;
    public List<BiomInfo> bioms = new List<BiomInfo>();

    public BiomInfo GetBiom(TileBase tile)
    {
        BiomInfo biom = bioms.Find(b => b.tile == tile);
        return biom;
    }

    #region Noise

    public float perlin(float x, float y) => Mathf.PerlinNoise((x + seed) / scale, (y + seed) / scale);

    public TileBase GetTile(float x, float y, float xSize, float ySize)
    {
        TileBase tile = null;
        float value = perlin(x / xSize, y / ySize);
        float ratioOffset = 0;

        int totalRatio = 0;
        bioms.ForEach(info => totalRatio += info.ratio);

        for(int i = 0; i < bioms.Count; i++)
        {
            BiomInfo info = bioms[i];

            float ratio = Mathf.Lerp(0, 1, (float)info.ratio / (float)totalRatio) + ratioOffset;
            ratioOffset = ratio;

            if (value <= ratio)
            {
                tile = info.tile;
                break;
            }
        }

        return tile;
    }

    public void SetSeed(int seed)
    {
        this.seed = seed;
    }


    #endregion
}

[Serializable]
public struct BiomInfo
{
    public TileBase tile;
    public List<ResourceDistribution> resource;
    [Space(20)]
    public int ratio;
}

[Serializable]
public struct ResourceDistribution
{
    public ResourceInfoSO resourceInfo; // 어떤 광물이
    public int maxResourceCnt;          // 몇개까지
    public float ratio;                 // 얼마나 나오는가?
}