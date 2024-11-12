using ResourceSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace BuildingManage
{
    public class RandomMapGenerator : MonoBehaviour
    {
        [SerializeField] private MapInfoSO _mapInfo;
        [SerializeField] private ResourceInfoGroupSO _resourceInfoGroup;

        public void GenerateMap(int seed)
        {
            SetBiom(seed);
            SetFabric();
        }

        /// <summary>
        /// 시드를 받아서 바닥타일을 깔아줌
        /// 시드 값에 따라서 바닥 패턴이 달라지니 처음에만 시드를 랜덤으로해주면 됨
        /// </summary>
        /// <param name="seed"></param>
        public void SetBiom(int seed)
        {
            BiomSO biom = _mapInfo.biomInfo;
            Vector2Int mapSize = _mapInfo.mapSize;
            Tilemap map = MapManager.Instance.FloorTile;

            biom.SetSeed(seed);

            //여기서부터 바닥을 깔아 주는거임
            for (int i = 0; i <= mapSize.x; i++)
            {
                for (int j = 0; j <= mapSize.y; j++)
                {
                    int x = i - mapSize.x / 2;
                    int y = j - mapSize.y / 2;

                    map.SetTile(new Vector3Int(x, y, 0), biom.GetTile(x, y, mapSize.x, mapSize.y));
                }
            }

            int chunkSize = biom.chunkSize;
            for (int i = -mapSize.x / 2; i < mapSize.x / 2; i += chunkSize)
            {
                for (int j = -mapSize.y / 2; j < mapSize.y; j += chunkSize)
                {
                    SetChunkResource(biom, i, j);
                }
            }
        }

        /// <summary>
        /// 건물을 랜덤으로 배치해주는 거임
        /// 얘는 최초로 배치 할 때 말고는 절대 안쓰일 예정임
        /// </summary>
        private void SetFabric()
        {
            List<Building> fabrics = new List<Building>();
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
                            minX + Mathf.CeilToInt(fabricInfo.fabricOffset),
                            -minX - Mathf.CeilToInt(fabricInfo.fabricOffset));

                        int posY = Random.Range(
                            minY + Mathf.CeilToInt(fabricInfo.fabricOffset),
                            -minY - Mathf.CeilToInt(fabricInfo.fabricOffset));

                        Vector2Int position = new Vector2Int(posX, posY);
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
                            MapManager.Instance.BuildController.Build(fabricInfo.fabric, position, true);
                            canNotSpawnArea.Add((position, fabricInfo.fabricOffset));
                            break;
                        }
                    }
                }
            });
        }

        public void SetChunkResource(BiomSO biom, int x, int y)
        {
            Tilemap resourceMap = MapManager.Instance.ResourceTile;

            int cx = x + GetRandomBySeed((int)biom.seed) % biom.chunkSize;
            int cy = y + GetRandomBySeed((int)biom.seed) % biom.chunkSize;

        }

        private int cur = 0;
        //시드 값으로 같은 랜덤값이 나와야해서 직접 랜덤 함수 구현을 하겠다!
        public int GetRandomBySeed(int seed)
        {
            int r = (int)((seed / 1827346f + 7f * 10293871f) / (float)cur);
            cur += (int)(((3f / 8128725f) + 93f) * 374f);
            return r;
        }
    }
}
