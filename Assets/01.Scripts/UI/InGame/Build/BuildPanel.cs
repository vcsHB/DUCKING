using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UI;
using UnityEngine;

namespace UI.InGame.Build
{
    
    public class BuildPanel : UIPanel
    {
        [SerializeField] private float _defaultXPos;
        [SerializeField] private float _activeXPos;
        private RectTransform _rectTrm;

        protected override void Awake()
        {
            base.Awake();
            _rectTrm = transform as RectTransform;
        }

        public override void Open()
        {
            base.Open();
            _rectTrm.DOAnchorPosX(_activeXPos, _activeDuration);

        }

        public override void Close()
        {
            base.Close();
            _rectTrm.DOAnchorPosX(_defaultXPos, _activeDuration);
        }
    }

}