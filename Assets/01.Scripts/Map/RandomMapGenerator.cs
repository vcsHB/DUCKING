using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace BuildingManage
{
    public class RandomMapGenerator : MonoBehaviour
    {
        [SerializeField] private MapInfoSO _mapInfo;


        public void GenerateMap(int seed)
        {
            SetFloor(seed);
            SetFabric();
        }

        /// <summary>
        /// �õ带 �޾Ƽ� �ٴ�Ÿ���� �����
        /// �õ� ���� ���� �ٴ� ������ �޶����� ó������ �õ带 �����������ָ� ��
        /// </summary>
        /// <param name="seed"></param>
        public void SetFloor(int seed)
        {
            _mapInfo.floorInfo.SetSeed(seed);

            for (int i = 0; i <= _mapInfo.mapSize.x; i++)
            {
                for (int j = 0; j <= _mapInfo.mapSize.y; j++)
                {
                    int x = i - _mapInfo.mapSize.x / 2;
                    int y = j - _mapInfo.mapSize.y / 2;

                    Tilemap map = MapManager.Instance.FloorTile;
                    map.SetTile(new Vector3Int(x, y, 0), _mapInfo.floorInfo.GetTile(x, y, _mapInfo.mapSize.x, _mapInfo.mapSize.y));
                }
            }
        }

        /// <summary>
        /// �ǹ��� �������� ��ġ���ִ� ����
        /// ��� ���ʷ� ��ġ �� �� ����� ���� �Ⱦ��� ������
        /// </summary>
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
                            MapManager.Instance.BuildingController.Build(fabricInfo.fabric, position);
                            canNotSpawnArea.Add((position, fabricInfo.fabricOffset));
                            break;
                        }
                    }
                }
            });
        }
    }
}
