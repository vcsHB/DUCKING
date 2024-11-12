using UnityEngine.UIElements;

namespace BuildingManage
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        private UnityEditor.Editor editor;
        private UnityEditor.Editor conditionEditor;

        public InspectorView()
        {
        }

        public void UpdateSelection(BuildingSO building)
        {
            Clear();

            if (building == null) return;

            UnityEngine.Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(building);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (editor != null && editor.target != null)
                {
                    editor.OnInspectorGUI();
                }
            });
            Add(container);
        }

        public void ClearSelection()
        {
            Clear();
        }
    }
}
