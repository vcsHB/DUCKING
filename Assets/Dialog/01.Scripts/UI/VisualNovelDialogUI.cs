using UnityEngine;
using Dialog;
using DG.Tweening;

namespace Dialog
{
    public class VisualNovelDialogUI : MonoBehaviour, IOpenCloseUI
    {
        private RectTransform _rectTrm;
        private float _easingTime = 0.5f;

        private void Awake()
        {
            _rectTrm = transform as RectTransform;
        }

        public void Open()
        {
            _rectTrm.DOAnchorPosY(0, _easingTime);
        }

        public void Close()
        {
            _rectTrm.DOAnchorPosY(-1080, _easingTime);
        }
    }
}
