using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
    public class NormalNodeSO : NodeSO
    {
        public string reader;
        public TextInfo contents;

        [HideInInspector] public NodeSO nextNode;

        public string GetReaderName() => reader;
        public string GetContents() => contents.text;

        private void OnEnable()
        {
            if (contents == null) contents = new TextInfo(this);
        }

        public override List<TagAnimation> GetAllAnimations()
        {
            return contents.GetAnimations();
        }
    }

    [Serializable]
    public class ImageStruct
    {
        public Sprite image;

        public Vector2 position;
        public Vector2 size;
    }
}

