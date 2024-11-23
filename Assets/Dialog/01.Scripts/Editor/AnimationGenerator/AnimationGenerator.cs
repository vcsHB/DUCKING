#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationGenerator : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Tools/AnimationGenerator")]
    public static void ShowExample()
    {
        AnimationGenerator wnd = GetWindow<AnimationGenerator>();
        wnd.titleContent = new GUIContent("AnimationGenerator");
    }

    public void CreateGUI()
    {

    }
}

#endif