using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class UIPanel : MonoBehaviour, IWindowPanel
    {
        protected CanvasGroup _canvasGroup;
        [SerializeField] protected float _activeDuration = 1f;
        public UnityEvent OnOpenEvent;
        public UnityEvent OnCloseEvent;
        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }


        public virtual void Open()
        {
            SetCanvasGroupActive(true);
        }

        public virtual void Close()
        {
            SetCanvasGroupActive(false);
        }

        protected void SetCanvasGroupActive(bool value)
        {
            _canvasGroup.DOFade(value ? 1f : 0, _activeDuration);
            _canvasGroup.interactable = value;
            _canvasGroup.blocksRaycasts = value;
        }
    }
}