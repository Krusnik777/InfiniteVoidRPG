using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons
{
    public abstract class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [field: SerializeField] public bool Interactable { get; private set; } = true;
        [SerializeField] private bool m_playPressAnimation = true;
        [SerializeField] private SelectableTransition m_selectableTransition;

        public Subject<UIButton> OnPress { get; private set; } = new();

        private const float _pressScale = -0.225f;
        private const float _pressAnimationDuration = 0.3f;

        private Tween scaleAnimation;
        private Tween pushAnimation;

        private bool _selected = false;

        public virtual void SetInteractable(bool state) => Interactable = state;

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (!Interactable) return;

            HandleOnPointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //if (!Interactable) return;

            HandleOnPointerExit();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            HandleOnPointerClick();
        }

        public void Reset() // maybe temp
        {
            if (m_selectableTransition.Type == SelectableTransition.SelectType.Animation)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                ChangeVisual(false);
            }
        }

        protected virtual void HandleOnPointerEnter()
        {
            ChangeVisual(true);
        }

        protected virtual void HandleOnPointerExit()
        {
            ChangeVisual(false);
        }

        protected virtual void HandleOnPointerClick()
        {
            if (!Interactable) return;

            if (m_playPressAnimation && pushAnimation == null)
            {
                pushAnimation?.Kill(true);
                pushAnimation = transform.DOPunchScale(Vector3.one * _pressScale, _pressAnimationDuration, 7).SetEase(Ease.InBack);
                pushAnimation.SetLink(gameObject).OnComplete(() => pushAnimation = null);
            }

            OnPress.OnNext(this);
        }

        protected void ChangeVisual(bool state)
        {
            if (state == _selected) return;

            _selected = state;

            switch (m_selectableTransition.Type)
            {
                case SelectableTransition.SelectType.Indicator:
                    m_selectableTransition.SelectedIndicator.SetActive(state);
                    break;

                case SelectableTransition.SelectType.Color:
                    m_selectableTransition.TargetGraphic.color = state ? m_selectableTransition.SelectedColor : m_selectableTransition.UnselectedColor;
                    break;

                case SelectableTransition.SelectType.Animation:
                    scaleAnimation?.Kill();
                    scaleAnimation = transform.DOScale(state ? m_selectableTransition.TargetScale : 1f, m_selectableTransition.AnimationTime).SetEase(Ease.InSine);
                    scaleAnimation.SetLink(gameObject);
                    break;
            }
        }
    }
}
