using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.InGame.Build
{
    public class BuildMaterialPanel : UIPanel
    {
        [SerializeField] private BuildMaterialSlot _slotPrefab;
        private Queue<BuildMaterialSlot> _slotPool = new Queue<BuildMaterialSlot>();

        private void Awake()
        {
            
        }

        public void SelectBuild()
        {
            
        }
    }
}