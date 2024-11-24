using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UI.InGame
{

    public class GameCanvasGroup : MonoBehaviour
    {
        
        [SerializeField] private UIPanel[] _defaultUIs;
        [SerializeField] private UIPanel[] _cinematicDisablePanels;

        public void SetActiveDefaultUIs()
        {
            for(int i = 0; i < _defaultUIs.Length; i++)
            {
                _defaultUIs[i].Open();
            }
        }

    }

}