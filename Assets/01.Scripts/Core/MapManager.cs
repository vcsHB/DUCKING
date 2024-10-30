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
        [SerializeField] private FabricSetSO _buildingSet;
        private List<Fabric> _buildingList = new List<Fabric>();

        #region ComponentRegion

        private RandomMapGenerator _mapGenerator;
        private FabricController _buildingController;

        public FabricController BuildingController => _buildingController;

        #endregion

        #region Save & Load

        private string _path = Path.Combine(Application.dataPath, "Saves/Map.json");
        private List<BuildingSave> _buildingSave = new List<BuildingSave>();
        private int _seed;

        #endregion

        public Tilemap FloorTile => _floorTileMap;

        private void Awake()
        {
            _floorTileMap.CompressBounds();

            _mapGenerator = GetComponent<RandomMapGenerator>();
            _buildingController = GetComponent<FabricController>();

            _buildingController.Init(_buildingSet);
            Load();
        }

        //public bool CheckBuildingOverlap(Fabric a, Fabric b)
        //{

        //}

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
        /// Find Fabric In World Position
        /// </summary>
        /// <param name="pos">World Position To Find Fabric</param>
        /// <param name="building"></param>
        /// <returns>Is Fabric Exsist</returns>
        public bool TryGetBuilding(Vector2 pos, out Fabric building)
        {
            Vector2Int tilePos = GetTilePos(pos);
            building = null;

            for (int i = 0; i < _buildingList.Count; i++)
            {
                if (_buildingList[i].CheckPosition(tilePos))
                {
                    building = _buildingList[i];
                }
            }

            return (building != null);
        }

        /// <summary>
        /// Find Fabric In TileMap Position
        /// </summary>
        /// <param name="pos">Tile Position To Find Fabric</param>
        /// <param name="building"></param>
        /// <returns>Is Fabric Exsist</returns>
        public bool TryGetBuilding(Vector2Int pos, out Fabric building)
        {
            building = null;

            for (int i = 0; i < _buildingList.Count; i++)
            {
                if (_buildingList[i].CheckPosition(pos))
                {
                    building = _buildingList[i];
                }
            }

            return (building != null);
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
                FabricEnum buildingType = Enum.Parse<FabricEnum>(building.name);
                Vector2Int position = new Vector2Int(building.posX, building.posY);

                _buildingController.Build(buildingType, position);
            });
        }

        public void AddBuilding(Fabric building, bool save = true)
        {
            _buildingList.Add(building);

            BuildingSave buildingInfo = new BuildingSave();
            buildingInfo.name = building.BuildingType.ToString();
            buildingInfo.posX = building.Position.x;
            buildingInfo.posY = building.Position.y;

            if (save)
            {
                _buildingSave.Add(buildingInfo);
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