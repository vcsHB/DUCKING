using UnityEngine.UIElements;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System;

namespace BuildingManage
{
    public class ScrollContents : Label
    {
        public new class UxmlFactory : UxmlFactory<ScrollContents, Label.UxmlTraits> { }

        public new class UxmlTraits : Label.UxmlTraits { }

        private FabricSO _building;
        private InspectorView _inspector;

        public FabricSO Building => _building;

        public ScrollContents()
        {
        }

        public ScrollContents(FabricSO building, InspectorView inspector)
        {
            _building = building;
            _inspector = inspector;

            Debug.Log(building.fabricType);
            text = building.fabricType.ToString();
            RegisterCallback<ClickEvent>(OnClick);
            name = "content";
        }

        private void OnClick(ClickEvent evt)
        {
            if (ClassListContains("select"))
            {
                RemoveFromClassList("select");
                _inspector.Clear();
                return;
            }

            foreach (var child in parent.Children())
            {
                child.RemoveFromClassList("select");
            }

            AddToClassList("select");
            _inspector.UpdateSelection(_building);
        }
    }
}