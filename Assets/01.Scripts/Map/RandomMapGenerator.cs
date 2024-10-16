using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

public class RandomMapGenerator : MonoBehaviour
{
    [SerializeField] private MapInfoSO _mapInfo;

    private void Awake()
    {
        GenerateMap();
    }

    private IEnumerator DelayGenerateMap()
    {
        yield return new WaitForSeconds(0.1f);
        SetFloor();
        SetFabric();
    }

    private void GenerateMap()
    {
        StartCoroutine(DelayGenerateMap());
    }

    private void SetFloor()
    {
        for (int i = 0; i <= _mapInfo.mapSize.x; i++)
        {
            for (int j = 0; j <= _mapInfo.mapSize.y; j++)
            {
                int x = i - _mapInfo.mapSize.x / 2;
                int y = j - _mapInfo.mapSize.y / 2;

                Tilemap map = MapManager.Instance.TileMap;
                map.SetTile(new Vector3Int(x, y, 0), _mapInfo.floorInfo.GetTile(x, y, _mapInfo.mapSize.x, _mapInfo.mapSize.y));
            }
        }
    }

    private void SetFabric()
    {
        List<Fabric> fabrics = new List<Fabric>();
        _mapInfo.fabrics.ForEach(fabricInfo =>
        {
            int minX = -_mapInfo.mapSize.x / 2;
            int minY = -_mapInfo.mapSize.y / 2;

            int spawnCnt = Random.Range(fabricInfo.minSpawnCnt, fabricInfo.maxSpawnCnt);

            List<(Vector2, float)> canNotSpawnArea = new List<(Vector2, float)>();

            for (int i = 0; i < spawnCnt; i++)
            {
                int trial = 10;

                while (trial-- > 0)
                {
                    int posX = Random.Range(
                        minX + Mathf.CeilToInt(fabricInfo.fabricRadius),
                        -minX - Mathf.CeilToInt(fabricInfo.fabricRadius));

                    int posY = Random.Range(
                        minY + Mathf.CeilToInt(fabricInfo.fabricRadius),
                        -minY - Mathf.CeilToInt(fabricInfo.fabricRadius));

                    Vector2 position = MapManager.Instance.GetWorldPos(new Vector2Int(posX, posY));
                    bool canSetFabric = true;

                    canNotSpawnArea.ForEach(value =>
                    {
                        Vector2 v = value.Item1;
                        float radius = value.Item2;

                        if ((position - v).magnitude <= radius)
                        {
                            canSetFabric = false;
                            return;
                        }
                    });

                    if (canSetFabric)
                    {
                        Instantiate(fabricInfo.fabricPf, position, Quaternion.identity);
                        canNotSpawnArea.Add((position, fabricInfo.fabricOffset));
                        break;
                    }
                }
            }
        });
    }
}
