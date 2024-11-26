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
        private CorrosiumController _corrosium;
        private System.Random _random;

        public void GenerateMap(int seed)
        {
            _corrosium = GetComponent<CorrosiumController>();

            SetBiom(seed);
            _corrosium.SetRandomEncorrosiveArea(seed);
            //SetFabric();
            //SetWall();

        }

        /// <summary>
        /// 시드를 받아서 바이옴을 생성해줌
        /// 1. 바닥타일을 깐다.
        /// 2. 바이옴에 따라 나오는 광맥을 생성한다.
        /// 시드 값에 따라서 바닥 패턴이 달라지니 처음에만 시드를 랜덤으로해주면 됨
        /// </summary>
        /// <param name="seed"></param>
        public void SetBiom(int seed)
        {
            _random = new System.Random((int)_mapInfo.biomInfo.seed);
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
                for (int j = -mapSize.y / 2; j < mapSize.y / 2; j += chunkSize)
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

                int spawnCnt = _random.Next(fabricInfo.minSpawnCnt, fabricInfo.maxSpawnCnt);

                List<(Vector2, float)> canNotSpawnArea = new List<(Vector2, float)>();

                for (int i = 0; i < spawnCnt; i++)
                {
                    int trial = 10;

                    while (trial-- > 0)
                    {
                        int posX = _random.Next(
                            minX + Mathf.CeilToInt(fabricInfo.fabricOffset),
                            -minX - Mathf.CeilToInt(fabricInfo.fabricOffset));

                        int posY = _random.Next(
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
                            MapManager.Instance.BuildController.TryBuild(fabricInfo.fabric, position, true);
                            canNotSpawnArea.Add((position, fabricInfo.fabricOffset));
                            break;
                        }
                    }
                }
            });
        }

        //청크에서 자원 생성
        public void SetChunkResource(BiomSO biomSO, int x, int y)
        {
            Tilemap floorMap = MapManager.Instance.FloorTile;
            Tilemap resourceMap = MapManager.Instance.ResourceTile;
            List<BiomInfo> biomList = new List<BiomInfo>();

            //현재 바이옴이 어떤게 있는지 구하는거임
            for (int i = 0; i < biomSO.chunkSize; i++)
            {
                for (int j = 0; j < biomSO.chunkSize; j++)
                {
                    TileBase tile = floorMap.GetTile(new Vector3Int(x + i, y + j));

                    BiomInfo biomTemp = biomSO.GetBiom(tile);
                    if (!biomList.Contains(biomTemp)) biomList.Add(biomTemp);
                }
            }

            // 현재 확인하고 있는 청크에 있는 바이옴에서
            biomList.ForEach(biom =>
            {
                //바이옴 안에 나오는 자원 광맥의 정보를 찾아서 생성을 시도해!
                for (int i = 0; i < biom.resource.Count; i++)
                {
                    //이 청크에 자원이 나올 것인가?!
                    ResourceVein vein = biom.resource[i];
                    bool spawn = _random.Next(0, 10001) > vein.exsistPercent;
                    if (!spawn) continue;

                    //대충 10번 쯤 청크안에서 찍어보는데 
                    for (int trial = 0; trial < 10; trial++)
                    {
                        int currentX = x + _random.Next(0, biomSO.chunkSize + 1);
                        int currentY = y + _random.Next(0, biomSO.chunkSize + 1);

                        if (resourceMap.GetTile(new Vector3Int(currentX, currentY)) == null
                            && floorMap.GetTile(new Vector3Int(currentX, currentY)) == biom.tile)
                        {
                            CreateVein(biom, vein, currentX, currentY);
                            break;
                        }
                    }
                }
            });
        }

        //광맥 생성!
        private void CreateVein(BiomInfo biom, ResourceVein veinInfo, int x, int y)
        {
            Tilemap resourceMap = MapManager.Instance.ResourceTile;
            Tilemap floorMap = MapManager.Instance.FloorTile;
            int spawnResourceCnt = _random.Next(veinInfo.minSpawn, veinInfo.maxSpawn + 1);

            SetSingleResourceTile(veinInfo, biom.tile, x, y);
            
            for (int i = 1; i < spawnResourceCnt; i++)
            {
                int direction = _random.Next(0, 4);
                Vector3Int nextPos = new Vector3Int(
                    x + Direction.directionX[direction] * veinInfo.thickedness,
                    y + Direction.directionY[direction] * veinInfo.thickedness);

                //안됬을 때 다시 시도하는거 따윈 없다. 걍 다시 돌아가라 하나 날린거다.
                //못 설치할 가능성이 있어서 이렇게 한 것인
                if (resourceMap.GetTile(nextPos) == null &&
                    floorMap.GetTile(nextPos) == biom.tile)
                {
                    x = nextPos.x;
                    y = nextPos.y;
                    SetSingleResourceTile(veinInfo, biom.tile, x, y);
                }
                else break;
            }
        }

        //광물 생성(두께를 적용해서)
        private void SetSingleResourceTile(ResourceVein resource, TileBase biomTile, int x, int y)
        {
            Tilemap resourceMap = MapManager.Instance.ResourceTile;
            Tilemap floorMap = MapManager.Instance.FloorTile;

            for (int i = 0; i < resource.thickedness; i++)
            {
                for (int j = 0; j < resource.thickedness; j++)
                {
                    Vector3Int nextPos = new Vector3Int(x + i, y + j);

                    if (resourceMap.GetTile(nextPos) == null &&
                            floorMap.GetTile(nextPos) == biomTile)
                    {
                        resourceMap.SetTile(nextPos, resource.resourceInfo.resourceTile);
                    }
                }
            }
        }
    
        private void SetWall()
        {

        }
    }
}
