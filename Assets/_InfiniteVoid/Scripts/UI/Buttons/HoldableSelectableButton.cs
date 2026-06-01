using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class HoldableSelectableButton : SelectableButton, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Hold")]
        [SerializeField] private Image m_holdFillImage;
        [SerializeField] private float m_holdTime = 1f;

        private bool _holded;
        private Coroutine _holdCoroutine;
        private bool _completedHold;

        public void SetCompleted(bool state)
        {
            _completedHold = state;
            m_holdFillImage.fillAmount = state ? 1f : 0f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_parentContainer != null && !_parentContainer.Interactable) return;

            HandleStartHold();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            HandleStopHold();
        }

        protected override void OnEnable() // TEMP for tests ?
        {
            if (_parentContainer == null)
            {
                ChangeVisual(false);

                SetCompleted(false);
            }
        }

        protected override void HandleOnPointerClick() { }

        protected override Context CreateContext()
        {
            return new Context
            {
                SelectAction = ChangeVisual,
                PressAction = () => { },
                HoldStartAction = HandleStartHold,
                HoldStopAction = HandleStopHold,
                ChooseNeighbourAction = GetNeighbour
            };
        }

        protected override SelectableButton GetNeighbour(NavigationDirection direction)
        {
            if (_holded) return null;

            return base.GetNeighbour(direction);
        }

        private void HandleStartHold()
        {
            _holded = true;

            if (_completedHold) return;

            StopAllCoroutines();
            _holdCoroutine = StartCoroutine(HoldRoutine());
        }

        private void HandleStopHold()
        {
            if (_holdCoroutine != null && !_completedHold)
            {
                StopCoroutine(_holdCoroutine);
                _holdCoroutine = null;

                SetCompleted(false);
            }

            _holded = false;
        }

        private IEnumerator HoldRoutine()
        {
            var elapsed = 0.0f;

            while (elapsed < m_holdTime)
            {
                m_holdFillImage.fillAmount = elapsed / m_holdTime;
                elapsed += Time.deltaTime;

                yield return null;
            }

            SetCompleted(true);

            OnPress.OnNext(this);

            _holded = false;
        }
    }
}
