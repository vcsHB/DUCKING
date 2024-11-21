using DG.Tweening;
using UnityEngine;
namespace UI
{

    public class MovePanel : UIPanel
    {
        [SerializeField] protected bool _isHorizontal;
        [SerializeField] protected float _defaultPostion;
        [SerializeField] protected float _activePosition;
        protected RectTransform _rectTrm;

        protected override void Awake()
        {
            base.Awake();
            _rectTrm = transform as RectTransform;
        }

        public override void Open()
        {
            base.Open();
            if (_isHorizontal)
                _rectTrm.DOAnchorPosX(_activePosition, _activeDuration);
            else
                _rectTrm.DOAnchorPosY(_activePosition, _activeDuration);

        }

        public override void Close()
        {
            base.Close();

            if (_isHorizontal)
                _rectTrm.DOAnchorPosX(_defaultPostion, _activeDuration);
            else
                _rectTrm.DOAnchorPosY(_defaultPostion, _activeDuration);

        }


    }
}