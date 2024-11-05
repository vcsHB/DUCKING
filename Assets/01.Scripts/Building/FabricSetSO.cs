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
    public class FabricSetSO : ScriptableObject
    {
        public List<FabricSO> buildings;
        private string _path = Path.Combine(Application.dataPath, "01.Scripts/Building/FabricEnum.cs");

        public void DeleteBuilding(FabricSO building)
        {
            buildings.Remove(building);

            StringBuilder sr = new StringBuilder();
            sr.Append("public enum FabricEnum { ");
            foreach (FabricEnum e in Enum.GetValues(typeof(FabricEnum)))
            {
                if (e == FabricEnum.None || 
                    e.ToString() == building.fabricTypeStr) continue;

                sr.Append($"{e.ToString()}, ");
            }
            sr.Append("None }");

            File.WriteAllText(_path, sr.ToString());
            AssetDatabase.Refresh();

            AssetDatabase.RemoveObjectFromAsset(building);
            AssetDatabase.SaveAssets();
        }

        public FabricSO CreateBulilding(string name)
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

            FabricSO building = ScriptableObject.CreateInstance<FabricSO>();
            building.name = name;

            if (Enum.TryParse(name, out FabricEnum b))
            {
                building.fabricTypeStr = name;
                building.fabricType = b;
                buildings.Add(building);

                AssetDatabase.AddObjectToAsset(building, this);
                AssetDatabase.SaveAssets();
                return building;
            }

            //Enum파일에다가 Enum을 추가해주는 작업을 해줄거임
            StringBuilder sr = new StringBuilder();
            sr.Append("public enum FabricEnum { ");
            foreach (FabricEnum e in Enum.GetValues(typeof(FabricEnum)))
            {
                if (e == FabricEnum.None) continue;
                sr.Append($"{e.ToString()}, ");
            }
            sr.Append($"{name}, ");
            sr.Append("None }");

            File.WriteAllText(_path, sr.ToString());
            AssetDatabase.Refresh();

            building.fabricTypeStr = name;
            buildings.Add(building);

            AssetDatabase.AddObjectToAsset(building, this);
            AssetDatabase.SaveAssets();
            return building;
        }

        public FabricSO FindBuilding(FabricEnum buildingEnum)
        {
            FabricSO building
                = buildings.Find(b => b.fabricType == buildingEnum);
            return building;
        }
    }
}
