using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;

namespace BuildingManage
{
    [RequireComponent(typeof(RandomMapGenerator))]
    public class MapManager : MonoSingleton<MapManager>
    {
        [SerializeField] private Tilemap _floorTileMap;
        [SerializeField] private BuildingSetSO _buildingSet;

        #region ComponentRegion

        private RandomMapGenerator _mapGenerator;
        private BuildingController _buildingController;

        public BuildingController BuildingController => _buildingController;

        #endregion

        #region Save & Load

        private string _path = Path.Combine(Application.dataPath, "Saves/Map.json");
        private int _seed;
        private List<BuildingSave> _buildingList = new List<BuildingSave>();

        #endregion

        public Tilemap FloorTile => _floorTileMap;

        private void Awake()
        {
            _floorTileMap.CompressBounds();

            _mapGenerator = GetComponent<RandomMapGenerator>();
            _buildingController = GetComponent<BuildingController>();

            _buildingController.Init(_buildingSet);
            Load();
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

        #endregion


        #region Save & Load

        public void Save()
        {
            MapSave save = new MapSave();

            save.floorSeed = _seed;
            save.buildings = _buildingList;

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
            _buildingList = save.buildings;

            _mapGenerator.SetFloor(_seed);
            _buildingList.ForEach(building =>
            {
                BuildingEnum buildingType = Enum.Parse<BuildingEnum>(building.name);
                Vector2Int position = new Vector2Int(building.posX, building.posY);

                _buildingController.Build(buildingType, position);
            });
        }

        public void AddBuilding(BuildingEnum buildingType, Vector2Int position)
        {
            BuildingSave buildingInfo = new BuildingSave();
            buildingInfo.name = buildingType.ToString();
            buildingInfo.posX = position.x;
            buildingInfo.posY = position.y;

            BuildingSave save = _buildingList.Find(
                building => building.posX == position.x &&
                            building.posY == position.y);
            if (save != null) return;

            _buildingList.Add(buildingInfo);
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