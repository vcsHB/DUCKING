using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class UIPanel : MonoBehaviour, IWindowPanel
    {
        protected CanvasGroup _canvasGroup;
        [SerializeField] protected float _fadeDuration = 1f;
        public UnityEvent OnOpenEvent;
        public UnityEvent OnCloseEvent;
        protected void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }


        public virtual void Open()
        {
            _canvasGroup.DOFade(1f, _fadeDuration);
        }

        public virtual void Close()
        {
            _canvasGroup.DOFade(0f, _fadeDuration);
        }
    }
}