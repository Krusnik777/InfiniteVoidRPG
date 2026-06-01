using System;
using InfiniteVoidRPG.Game.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Buttons
{
    public class InputButton : UIButton, IDisposable
    {
        [Header("Input")]
        [SerializeField] protected InputActionReference m_inputActionReference;

        protected InputAction _bindedAction;

        public override void ResetState()
        {
            base.ResetState();

            Dispose();
        }

        public virtual void Dispose()
        {
            if (_bindedAction != null)
            {
                _bindedAction.performed -= OnActionPerformed;
            }
        }

        public virtual void Init(GameInputService gameInputService)
        {
            _bindedAction = gameInputService.ActionsAsset.FindAction(m_inputActionReference.action.name);

            if (_bindedAction == null) throw new NullReferenceException($"Not found asset for reference: {m_inputActionReference.asset.name}");

            _bindedAction.performed += OnActionPerformed;
        }

        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            HandleOnPointerClick();
        }
    }
}
