using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InfiniteVoidRPG.Game.Services
{
    public class UIInputControlledEntity
    {
        public object Sender { get; private set; }
        public Action OnSubmit;
        public Action<Vector2> OnMove;
        public Action OnCancel;

        public UIInputControlledEntity(object sender)
        {
            Sender = sender;
        }
    }

    public class UIInputController : IDisposable
    {
        public Subject<Unit> OnUISubmitPressed { get; private set; } = new();

        private GameInput _gameInput;
        private UIInputControlledEntity _controlledEntity;

        public UIInputController(GameInput gameInput)
        {
            _gameInput = gameInput;
            _gameInput.UIControls.Enable();

            _gameInput.UIControls.Submit.performed += OnSubmit;
            _gameInput.UIControls.Move.performed += OnMove;
            _gameInput.UIControls.Cancel.performed += OnCancel;
        }

        public void Dispose()
        {
            _gameInput.UIControls.Submit.performed -= OnSubmit;
            _gameInput.UIControls.Move.performed -= OnMove;
            _gameInput.UIControls.Cancel.performed -= OnCancel;
        }

        public void AssignControlledEntity(object sender, UIInputControlledEntity entity)
        {
            if (entity == null && !(_controlledEntity != null && sender == _controlledEntity.Sender)) return;

            _controlledEntity = entity;
        }

        private void OnSubmit(InputAction.CallbackContext context)
        {
            if (_controlledEntity == null)
            {
                OnUISubmitPressed?.OnNext(Unit.Default);
            }
            else
            {
                _controlledEntity?.OnSubmit?.Invoke();
            }
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            var value = _gameInput.UIControls.Move.ReadValue<Vector2>();

            _controlledEntity?.OnMove?.Invoke(value);
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            _controlledEntity?.OnCancel?.Invoke();
        }
    }
}
