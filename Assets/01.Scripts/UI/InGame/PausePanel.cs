using DG.Tweening;
using InputManage;
using UnityEngine;

namespace UI.InGame
{
    public class PausePanel : UIPanel
    {
        [SerializeField] private UIInputReaderSO _inputReader;
        [SerializeField] private float _defaultYPos;
        [SerializeField] private float _activeYPos;
        private RectTransform _rectTrm;

        protected override void Awake()
        {
            base.Awake();
            _rectTrm = transform as RectTransform;

            _inputReader.OnPauseEvent += HandleTogglePausePanel;
        }

        private void OnDestroy()
        {
            _inputReader.OnPauseEvent -= HandleTogglePausePanel;

        }

        public void HandleTogglePausePanel()
        {
            if (_isActive)
                Close();
            else
                Open();
        }

        public override void Open()
        {
            base.Open();
            _rectTrm.DOAnchorPosY(_activeYPos, _activeDuration);
        }

        public override void Close()
        {
            base.Close();
            _rectTrm.DOAnchorPosY(_defaultYPos, _activeDuration);
        }


        public void HandleSelectContinue()
        {
            Close();
        }

        public void HandleSelectSetting()
        {

        }

        public void HandleSelectExit()
        {

        }


    }

}