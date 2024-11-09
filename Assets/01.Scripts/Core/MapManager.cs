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
        [SerializeField] private BuildingSetSO _buildingSet;

        private List<Building> BuildingList = new List<Building>();
        private Dictionary<Vector2Int, Building> _buildings = new();

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
            _floorTileMap.CompressBounds();

            _mapGenerator = GetComponent<RandomMapGenerator>();
            _buildController = GetComponent<BuildController>();

            _buildController.Init(_buildingSet);
            Load();
        }

        public bool CheckBuildingOverlap(BuildingSize size)
        {
            bool isOverlap = false;

            for (int i = size.min.x; i <= size.max.x; i++)
            {
                for (int j = size.min.y; j <= size.max.y; j++)
                {
                    if (_buildings.ContainsKey(new Vector2Int(i, j))) isOverlap = true;
                }
            }

            //BuildingList.ForEach(fabric =>
            //{
            //    if (fabric.Position.IsOverlap(size))
            //    {
            //        isOverlap = true;
            //        return;
            //    }
            //});

            return isOverlap;
        }

        #region PositionConvert

        /// <summary>
        /// 월드 포지션을 받아서 타일맵의 포지션으로 바꿔줌
        /// </summary>
        /// <param name="pos">WorldPosition</param>
        /// <returns>Position in tilemap</returns>
        public Vector2Int GetTilePos(Vector2 pos) => (Vector2Int)_floorTileMap.WorldToCell(pos);

        /// <summary>
        /// 타일맵의 포지션을 받아서 월드 포지션으로 바꿔줌
        /// </summary>
        /// <param name="pos">Position in tilemap</param>
        /// <returns>WorldPosition</returns>
        public Vector2 GetWorldPos(Vector2Int pos) => (Vector2)_floorTileMap.CellToWorld((Vector3Int)pos);

        /// <summary>
        /// 벡터값을 받아서 타일맵에 딱들어맞는 포지션으로 바꿔줌
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

            if (!_buildings.ContainsKey(tilePos))
            {
                building = null;
                return false;
            }

            building = _buildings[tilePos];
            return true;

            //building = null;

            //for (int i = 0; i < BuildingList.Count; i++)
            //{
            //    if (BuildingList[i].CheckPosition(tilePos))
            //    {
            //        building = BuildingList[i];
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
            if (!_buildings.ContainsKey(pos))
            {
                building = null;
                return false;
            }

            building = _buildings[pos];
            return true;
            //building = null;

            //for (int i = 0; i < BuildingList.Count; i++)
            //{
            //    if (BuildingList[i].CheckPosition(pos))
            //    {
            //        building = BuildingList[i];
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
            BuildingList.Add(building);

            for (int i = building.Position.min.x; i <= building.Position.max.x; i++)
            {
                for (int j = building.Position.min.y; j <= building.Position.max.y; j++)
                {
                    Vector2Int p = new Vector2Int(i, j);
                    if (_buildings.ContainsKey(p) == false) _buildings.Add(p, building);
                }
            }

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
            BuildingList.Remove(building);

            for (int i = building.Position.min.x; i <= building.Position.max.x; i++)
            {
                for (int j = building.Position.min.y; j <= building.Position.max.y; j++)
                {
                    _buildings.Remove(new Vector2Int(i, j));
                }
            }

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