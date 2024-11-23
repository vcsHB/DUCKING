using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dialog
{
    [Serializable]
    public class TextInfo
    {
        [TextArea(5, 20)]
        public string text = "";
        public Action OnSelectTextInfo;

        [HideInInspector] public List<TagAnimation> tagAnimations = new();
        [SerializeField] private NodeSO _node;

#if UNITY_EDITOR
        public void AddAnimation(TagAnimation tagAnim, int start, int end)
        {
            tagAnim.animStartPos = start;
            tagAnim.animLength = end - start;
            tagAnimations.Add(tagAnim);

            AssetDatabase.AddObjectToAsset(tagAnim, _node);
            EditorUtility.SetDirty(_node);
            AssetDatabase.SaveAssets();
        }

        public void RemoveAnimation(TagAnimation tagAnim)
        {
            tagAnimations.Remove(tagAnim);
            AssetDatabase.RemoveObjectFromAsset(tagAnim);
            EditorUtility.SetDirty(_node);
            AssetDatabase.SaveAssets();

        }
        public TextInfo(NodeSO node)
        {
            _node = node;
            EditorUtility.SetDirty(_node);
            AssetDatabase.SaveAssets();
        }

#endif
        public List<TagAnimation> GetAnimations() => tagAnimations;


    }
}
