
#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialog
{
    public  class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        private UnityEditor.Editor editor;
        public TagAnimation tagAnim;

        public InspectorView()
        {
        }

        public void UpdateSelection(NodeView node)
        {
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(node.nodeSO);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (editor.target != null)
                {
                    editor.OnInspectorGUI();
                }
            });
            Add(container);
        }

        public void UpdateSelection(ConditionSO condition)
        {
            Clear();

            if (condition == null) return;

            UnityEngine.Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(condition);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (editor.target != null)
                {
                    editor.OnInspectorGUI();
                }
            });
            Add(container);
        }

        public void UpdateSelection(TagEnum anim)
        {
            try
            {
                Type t = Type.GetType($"Dialog.{anim}TagAnimation");
                if(t == null) 
                {
                    Debug.Log(t);
                    t = Type.GetType($"{anim}TagAnimation");
                }
                Debug.Log($"{anim}TagAnimation");
                tagAnim = Activator.CreateInstance(t) as TagAnimation;

                UnityEngine.Object.DestroyImmediate(editor);
                editor = UnityEditor.Editor.CreateEditor(tagAnim);
                IMGUIContainer container = new IMGUIContainer(() =>
                {
                    if (editor.targets != null)
                    {
                        editor.OnInspectorGUI();
                    }
                });
                Add(container);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError($"{anim} tag animation not exsist");
            }
        }

        public void UpdateSelection(TagAnimation anim)
        {
            try
            {
                UnityEngine.Object.DestroyImmediate(editor);
                editor = UnityEditor.Editor.CreateEditor(anim);
                IMGUIContainer container = new IMGUIContainer(() =>
                {
                    if (editor.targets != null)
                    {
                        editor.OnInspectorGUI();
                    }
                });
                Add(container);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError($"{anim.tagType} tag animation not exsist");
            }
        }

        public void ClearSelection()
        {
            tagAnim = null;
            Clear();
        }
    }
}

#endif