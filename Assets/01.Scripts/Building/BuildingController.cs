using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingManage
{
    public class BuildingController : MonoBehaviour
    {
        private BuildingSetSO _buildingSet;
        private Transform _buildingParent;

        [SerializeField] private BuildingEnum testBuilding;

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector2Int position
                    = MapManager.Instance.GetTilePos(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                Build(testBuilding, position);
            }
        }

        public void Build(BuildingEnum building, Vector2Int position)
        {
            BuildingSO info = _buildingSet.FindBuilding(building);

            Vector2 worldPosition = MapManager.Instance.GetWorldPos(position);
            GameObject buildingInstance = Instantiate(info.prefab, worldPosition, Quaternion.identity);

            MapManager.Instance.AddBuilding
                (info.buildingType, position);
            MapManager.Instance.Save();
        }

        public void Init(BuildingSetSO buildingSet)
        {
            _buildingSet = buildingSet;
        }
    }
}

