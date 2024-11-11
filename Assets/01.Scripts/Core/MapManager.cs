using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

namespace BuildingManage
{
    [RequireComponent(typeof(RandomMapGenerator))]
    public class MapManager : MonoSingleton<MapManager>
    {
        [SerializeField] private Tilemap _floorTileMap;
        [SerializeField] private Tilemap _resourceTileMap;
        [SerializeField] private Tilemap _wallTileMap;
        [SerializeField] private BuildingSetSO _buildingSet;

        private RaycastHit2D[] _hit;
        [SerializeField] private LayerMask _whatIsBuilding;

        private List<Building> _buildingList = new List<Building>();

        #region ComponentRegion

        private RandomMapGenerator _mapGenerator;
        private BuildController _buildController;

        public BuildController BuildController => _buildController;

        #endregion

        #region Save & Load

        private string _path = Path.Combine(Application.dataPath, "Saves/Map.json");
        private List<BuildingSave> _buildingSave = new List<BuildingSave>();
        private int _seed;

        #endregion

        public Tilemap FloorTile => _floorTileMap;
        public Tilemap ResourceTile => _resourceTileMap;

        private void Awake()
        {
            PathFinder.Initialize(_wallTileMap);
            _floorTileMap.CompressBounds();

            _mapGenerator = GetComponent<RandomMapGenerator>();
            _buildController = GetComponent<BuildController>();
            _hit = new RaycastHit2D[1];

            _buildController.Init(_buildingSet);
            Load();
        }

        BuildingSize s;
        public bool CheckBuildingOverlap(BuildingSize size)
        {
            int cnt = Physics2D.BoxCastNonAlloc(size.center, Vector2.one * (size.size - 0.1f), 0, Vector2.zero, _hit, 1, _whatIsBuilding);
            s = size;

            return cnt > 0;
            //bool isOverlap = false;

            //for (int i = size.min.x; i <= size.max.x; i++)
            //{
            //    for (int j = size.min.y; j <= size.max.y; j++)
            //    {
            //        if (_buildings.ContainsKey(new Vector2Int(i, j))) isOverlap = true;
            //    }
            //}

            //_buildingList.ForEach(fabric =>
            //{
            //    if (fabric.Position.IsOverlap(size))
            //    {
            //        isOverlap = true;
            //        return;
            //    }
            //});

            //return isOverlap;
        }

        #region PositionConvert

        /// <summary>
        /// ���� �������� �޾Ƽ� Ÿ�ϸ��� ���������� �ٲ���
        /// </summary>
        /// <param name="pos">WorldPosition</param>
        /// <returns>Position in tilemap</returns>
        public Vector2Int GetTilePos(Vector2 pos) => (Vector2Int)_floorTileMap.WorldToCell(pos);

        /// <summary>
        /// Ÿ�ϸ��� �������� �޾Ƽ� ���� ���������� �ٲ���
        /// </summary>
        /// <param name="pos">Position in tilemap</param>
        /// <returns>WorldPosition</returns>
        public Vector2 GetWorldPos(Vector2Int pos) => (Vector2)_floorTileMap.CellToWorld((Vector3Int)pos);

        /// <summary>
        /// ���Ͱ��� �޾Ƽ� Ÿ�ϸʿ� �����´� ���������� �ٲ���
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector2 RoundToTilePos(Vector2 pos) => (Vector2)_floorTileMap.CellToWorld(_floorTileMap.WorldToCell(pos));

        /// <summary>
        /// Find Building In World Position
        /// </summary>
        /// <param name="pos">World Position To Find Building</param>
        /// <param name="building"></param>
        /// <returns>Is Building Exsist</returns>
        public bool TryGetBuilding(Vector2 pos, out Building building)
        {
            Vector2Int tilePos = GetTilePos(pos);

            for(int i = 0; i < _buildingList.Count; i++)
            {
                if (_buildingList[i].CheckPosition(tilePos))
                {
                    building = _buildingList[i]; 
                    return true;
                }
            }

            building = null;
            return false;

            //_buildingList.ForEach(building => building.CheckPosition(tilePos));

            //if (!_buildings.ContainsKey(tilePos))
            //{
            //    building = null;
            //    return false;
            //}

            //building = _buildings[tilePos];
            //return true;

            //building = null;

            //for (int i = 0; i < _buildingList.Count; i++)
            //{
            //    if (_buildingList[i].CheckPosition(tilePos))
            //    {
            //        building = _buildingList[i];
            //    }
            //}

            //return (building != null);
        }

        /// <summary>
        /// Find Building In TileMap Position
        /// </summary>
        /// <param name="pos">Tile Position To Find Building</param>
        /// <param name="building"></param>
        /// <returns>Is Building Exsist</returns>
        public bool TryGetBuilding(Vector2Int pos, out Building building)
        {
            for (int i = 0; i < _buildingList.Count; i++)
            {
                if (_buildingList[i].CheckPosition(pos))
                {
                    building = _buildingList[i];
                    return true;
                }
            }

            building = null;
            return false;

            //if (!_buildings.ContainsKey(pos))
            //{
            //    building = null;
            //    return false;
            //}

            //building = _buildings[pos];
            //return true;
            //building = null;

            //for (int i = 0; i < _buildingList.Count; i++)
            //{
            //    if (_buildingList[i].CheckPosition(pos))
            //    {
            //        building = _buildingList[i];
            //    }
            //}

            //return (building != null);
        }

        #endregion

        #region Save & Load

        public void Save()
        {
            MapSave save = new MapSave();

            save.floorSeed = _seed;
            save.buildings = _buildingSave;

            string json = JsonUtility.ToJson(save, true);
            File.WriteAllText(_path, json);
        }

        public void Load()
        {
            if (!File.Exists(_path))
            {
                _seed = Random.Range(-10000, 100001);
                _mapGenerator.GenerateMap(_seed);
                Save();

                return;
            }

            string json = File.ReadAllText(_path);
            MapSave save = JsonUtility.FromJson<MapSave>(json);

            _seed = save.floorSeed;
            _buildingSave = save.buildings;

            _mapGenerator.SetFloor(_seed);
            _buildingSave.ForEach(building =>
            {
                BuildingEnum buildingType = Enum.Parse<BuildingEnum>(building.name);
                Vector2Int position = new Vector2Int(building.posX, building.posY);

                _buildController.Build(buildingType, position, false);
            });
        }

        public void AddBuilding(Building building, bool save = true)
        {
            _buildingList.Add(building);

            if (save)
            {
                BuildingSave buildingSave = new BuildingSave();
                buildingSave.name = building.BuildingType.ToString();
                buildingSave.posX = building.Position.min.x;
                buildingSave.posY = building.Position.min.y;

                _buildingSave.Add(buildingSave);
            }
        }

        public void RemoveBuilding(Building building, bool save)
        {
            _buildingList.Remove(building);

            if (save)
            {
                BuildingSave buildingSave = new BuildingSave();
                buildingSave.name = building.BuildingType.ToString();
                buildingSave.posX = building.Position.min.x;
                buildingSave.posY = building.Position.min.y;

                _buildingSave.Remove(buildingSave);
            }
        }

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (s != null)
            {
                Gizmos.DrawWireCube(s.center, Vector2.one * s.size);
            }
        }
    }

    [Serializable]
    public class MapSave
    {
        public int floorSeed;
        public List<BuildingSave> buildings = new();
    }

    [Serializable]
    public class BuildingSave
    {
        public string name;
        public int posX, posY;
    }
}