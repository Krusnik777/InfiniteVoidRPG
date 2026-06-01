using System.Collections;
using InfiniteVoidRPG.Game.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class HoldableInputButton : InputButton, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Hold")]
        [SerializeField] private Image m_holdFillImage;
        [SerializeField] private float m_holdTime = 1f;

        //private bool _holded;
        private Coroutine _holdCoroutine;
        private bool _completedHold;

        public override void Dispose()
        {
            if (_bindedAction != null)
            {
                _bindedAction.started -= OnActionStarted;
                _bindedAction.canceled -= OnActionStopped;
            }

            if (_holdCoroutine != null && !_completedHold)
            {
                StopCoroutine(_holdCoroutine);
                _holdCoroutine = null;
            }

            //_holded = false;
        }

        public void SetCompleted(bool state)
        {
            _completedHold = state;
            m_holdFillImage.fillAmount = state ? 1f : 0f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandleStartHold();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            HandleStopHold();
        }

        public override void Init(GameInputService gameInputService)
        {
            _bindedAction = gameInputService.ActionsAsset.FindAction(m_inputActionReference.action.name);

            if (_bindedAction == null) throw new System.NullReferenceException($"Not found asset for reference: {m_inputActionReference.asset.name}");

            _bindedAction.started += OnActionStarted;
            _bindedAction.canceled += OnActionStopped;
        }

        protected override void HandleOnPointerClick() { }

        private void OnEnable() // TEMP for tests ?
        {
            ChangeVisual(false);

            SetCompleted(false);
        }

        private void OnActionStarted(InputAction.CallbackContext context)
        {
            HandleStartHold();
        }

        private void OnActionStopped(InputAction.CallbackContext context)
        {
            HandleStopHold();
        }

        private void HandleStartHold()
        {
            //_holded = true;

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

            //_holded = false;
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

            //_holded = false;
        }
    }
}
