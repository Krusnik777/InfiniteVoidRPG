using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace InfiniteVoidRPG.Game.Services
{
    public class GameInputService : IDisposable
    {
        public Subject<Unit> OnSelectablesSubmitPressed { get; private set;} = new();
        public Subject<Vector2> OnSelectablesMovePressed { get; private set;} = new();

        private GameInput _gameInput;
        public InputActionAsset ActionsAsset => _gameInput.asset;

        private IDisposable _anyButtonPressListenerDisposable;

        public GameInputService()
        {
            _gameInput = new();
            _gameInput.Enable();

            _gameInput.Selectables.Submit.performed += OnSelectablesSubmit;
            _gameInput.Selectables.Move.performed += OnSelectablesMove;
        }

        private void OnSelectablesSubmit(InputAction.CallbackContext context)
        {
            OnSelectablesSubmitPressed?.OnNext(Unit.Default);
        }

        private void OnSelectablesMove(InputAction.CallbackContext context)
        {
            var value = _gameInput.Selectables.Move.ReadValue<Vector2>();

            OnSelectablesMovePressed?.OnNext(value);
        }

        public void Dispose()
        {
            _anyButtonPressListenerDisposable?.Dispose();

            _gameInput.Selectables.Submit.performed -= OnSelectablesSubmit;
            _gameInput.Selectables.Move.performed -= OnSelectablesMove;
        }

        public void ClearReactionForAnyButtonPress() => _anyButtonPressListenerDisposable?.Dispose();

        public void SetReactionForAnyButtonPress(Action action)
        {
            _anyButtonPressListenerDisposable?.Dispose();

            _anyButtonPressListenerDisposable = InputSystem.onAnyButtonPress.Call(_ =>
            {
               _anyButtonPressListenerDisposable?.Dispose();

               action?.Invoke(); 
            });
        }
    }
}
