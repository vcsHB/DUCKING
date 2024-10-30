
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace BuildingManage
{
    public class FabricGeneratorEditor : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        private FabricSetSO _buildingSet;
        private VisualElement _container;
        private ScrollView _scrollView;
        private InspectorView _inspector;

        private Button _addBtn;
        private Button _removeBtn;
        private TextField _textField;

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is FabricSetSO buildingSet)
            {
                ShowEditor();
                return true;
            }
            return false;
        }

        [MenuItem("Tools/FabricGenerator")]
        public static void ShowEditor()
        {
            FabricGeneratorEditor wnd = GetWindow<FabricGeneratorEditor>();
            wnd.titleContent = new GUIContent("FabricGenerator");
        }


        public void CreateGUI()
        {
            if (Selection.activeObject is FabricSetSO buildingSet)
                _buildingSet = buildingSet;

            VisualElement root = rootVisualElement;

            VisualElement content = m_VisualTreeAsset.Instantiate();
            content.style.flexGrow = 1;
            root.Add(content);

            _scrollView = root.Q<ScrollView>("scroll-view");
            _inspector = root.Q<InspectorView>("inspector-view");
            _container = _scrollView.Q<VisualElement>
                ("unity-content-container");

            _addBtn = root.Q<Button>("add-btn");
            _removeBtn = root.Q<Button>("remove-btn");
            _textField = root.Q<TextField>("text-field");

            _addBtn.RegisterCallback<ClickEvent>(Add);
            _removeBtn.RegisterCallback<ClickEvent>(Remove);
        }

        private void Add(ClickEvent evt)
        {
            string buildingEnum = _textField.text;
            FabricSO building = _buildingSet.CreateBulilding(buildingEnum);

            AddScrollContent(building);
        }

        private void Remove(ClickEvent evt)
        {
            var content = _container.Q<ScrollContents>("content", "select");
            if (content == null)
            {
                Debug.LogWarning("There is nothing to remove");
                return;
            }

            _inspector.Clear();
            _inspector.UpdateSelection(null);

            _buildingSet.DeleteBuilding(content.Building);
            content.RemoveFromHierarchy();
        }

        private void OnSelectionChange()
        {
            while(_container.childCount > 0)
            {
                _container.RemoveAt(0);
            }

            if (Selection.activeObject is FabricSetSO buildingSet)
            {
                _buildingSet = buildingSet;

                if (buildingSet != null && AssetDatabase.CanOpenAssetInEditor(buildingSet.GetInstanceID()))
                {
                    buildingSet.buildings.ForEach(AddScrollContent);
                }
            }
        }

        private void AddScrollContent(FabricSO building)
        {
            ScrollContents contents = new ScrollContents(building, _inspector);
            contents.AddToClassList("scroll-content");

            _scrollView.Add(contents);
        }
    }

}