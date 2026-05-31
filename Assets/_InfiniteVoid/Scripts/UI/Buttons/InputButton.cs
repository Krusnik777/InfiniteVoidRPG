using System;
using InfiniteVoidRPG.Game.Services;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Buttons
{
    public class InputButton : UIButton, IDisposable
    {
        [Header("Input")]
        [SerializeField] private InputActionReference m_inputActionReference;

        private InputAction _bindedAction;

        public void Dispose()
        {
            if (_bindedAction != null)
            {
                _bindedAction.performed -= OnActionPerformed;
            }
        }

        public void Init(GameInputService gameInputService)
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
