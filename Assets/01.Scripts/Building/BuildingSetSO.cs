using BuildingManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BuildingManage
{
    [CreateAssetMenu(menuName = "SO/BuildingSet")]
    public class BuildingSetSO : ScriptableObject
    {
        public List<BuildingSO> buildings;
        private string _path = Path.Combine(Application.dataPath, "01.Scripts/Building/BuildingEnum.cs");

        public void DeleteBuilding(BuildingSO building)
        {
            buildings.Remove(building);

            StringBuilder sr = new StringBuilder();
            sr.Append("public enum BuildingEnum { ");
            foreach (BuildingEnum e in Enum.GetValues(typeof(BuildingEnum)))
            {
                if (e == BuildingEnum.None || 
                    e.ToString() == building.buildingTypeStr) continue;

                sr.Append($"{e.ToString()}, ");
            }
            sr.Append("None }");

            File.WriteAllText(_path, sr.ToString());
            AssetDatabase.Refresh();

            AssetDatabase.RemoveObjectFromAsset(building);
            AssetDatabase.SaveAssets();
        }

        public BuildingSO CreateBulilding(string name)
        {

            if (String.IsNullOrEmpty(name))
            {
                Debug.LogError("You have to write building name");
                return null;
            }

            if (name.Contains(' '))
            {
                Debug.LogError("You must not contain empty in name");
                return null;
            }

            BuildingSO building = ScriptableObject.CreateInstance<BuildingSO>();
            building.name = name;

            if (Enum.TryParse(name, out BuildingEnum b))
            {
                building.buildingTypeStr = name;
                building.fabricType = b;
                buildings.Add(building);

                AssetDatabase.AddObjectToAsset(building, this);
                AssetDatabase.SaveAssets();
                return building;
            }

            //Enum파일에다가 Enum을 추가해주는 작업을 해줄거임
            StringBuilder sr = new StringBuilder();
            sr.Append("public enum FabricEnum { ");
            foreach (BuildingEnum e in Enum.GetValues(typeof(BuildingEnum)))
            {
                if (e == BuildingEnum.None) continue;
                sr.Append($"{e.ToString()}, ");
            }
            sr.Append($"{name}, ");
            sr.Append("None }");

            File.WriteAllText(_path, sr.ToString());
            AssetDatabase.Refresh();

            building.buildingTypeStr = name;
            buildings.Add(building);

            AssetDatabase.AddObjectToAsset(building, this);
            AssetDatabase.SaveAssets();
            return building;
        }

        public BuildingSO FindBuilding(BuildingEnum buildingEnum)
        {
            BuildingSO building
                = buildings.Find(b => b.fabricType == buildingEnum);
            return building;
        }
    }
}
