using DG.Tweening;
using InputManage;
using UnityEngine;

namespace UI.InGame.Build
{
    public class BuildPanel : UIPanel
    {
        [SerializeField] private UIInputReaderSO _uiInputReader;
        [SerializeField] private float _defaultXPos;
        [SerializeField] private float _activeXPos;
        private RectTransform _rectTrm;

        protected override void Awake()
        {
            base.Awake();

            _uiInputReader.BuildEvent += HandleToggleBuildPanel;
            _rectTrm = transform as RectTransform;
        }

        private void OnDestroy() {
            _uiInputReader.BuildEvent -= HandleToggleBuildPanel;
        }

        private void HandleToggleBuildPanel()
        {
            if(_isActive)
                Close();
            else 
                Open();
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