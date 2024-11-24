using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI.TitleScene
{

    public class FadePanel : UIPanel
    {

        [SerializeField] private Image _upPanel;
        [SerializeField] private Image _downPanel;


        public override void Open()
        {
            _upPanel.DOFillAmount(1f, _activeDuration).SetUpdate(_isUnscaledTime);
            _downPanel.DOFillAmount(1f, _activeDuration).SetUpdate(_isUnscaledTime);
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public override void Close()
        {
            _upPanel.DOFillAmount(0f, _activeDuration).SetUpdate(_isUnscaledTime);
            _downPanel.DOFillAmount(0f, _activeDuration).SetUpdate(_isUnscaledTime);

            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

    }

}