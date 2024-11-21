using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.InGame.Build
{
    
    public class BuildTypeDescriptionPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI _descriptuonText;
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


        public void ShowDescription(string content)
        {
            _descriptuonText.text = content;
        }

        public void SetPanelActive(bool value)
        {
            if (value)
                Open();
            else
                Close();
        }

        public void SetPositionY(float y)
        {
            _rectTrm.anchoredPosition = 
                new Vector2(_rectTrm.anchoredPosition.x, 
                y - (_rectTrm.rect.height / 2));
        }
    }

}