
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace Dialog
{
    [CustomEditor(typeof(OptionNodeSO))]
    public class OptionNodeEditor : Editor
    {
        public SerializedProperty _options;
        public SerializedProperty _optionPf;
        public ReorderableList optionList;


        private int _selected = -1;
        private bool _isOptionOpen = false;

        private void OnEnable()
        {
            OptionNodeSO optionSO = (OptionNodeSO)target;

            optionList = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("options"),
                true, true, true, true);
            _optionPf = serializedObject.FindProperty("optionPf");
            _options = serializedObject.FindProperty("options");

            //15 + 5 + 70 + 5 + 15 + 5
            optionList.elementHeight = 140;
            optionList.drawElementCallback = (rect, index, active, focused) =>
            {
                var element = optionList.serializedProperty.GetArrayElementAtIndex(index);
                var optionProp = element.FindPropertyRelative("option");

                Rect labelRect = new Rect(rect.x, rect.y, rect.width, 15);
                Rect txtAreaRect = new Rect(rect.x, rect.y + 20, rect.width, 60);

                EditorGUI.LabelField(labelRect, $"Option-{index}");
                EditorGUI.PropertyField(txtAreaRect, optionProp, true);

            };

            optionList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Options");
            optionList.onAddCallback = list => optionSO.AddOption();
            optionList.onRemoveCallback = list =>
            {
                if (optionSO.options.Count >= 0)
                {
                    //선택된게 없으면
                    if (list.index == -1)
                    {
                        //제일 뒤에있는거를 지워라
                        optionSO.RemoveOption(optionSO.options.Count - 1);
                    }
                    else
                    {
                        //선택된거를 지워라
                        optionSO.RemoveOption(list.index);
                    }
                }
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            optionList.DoLayoutList();
            EditorGUILayout.PropertyField(_optionPf);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif