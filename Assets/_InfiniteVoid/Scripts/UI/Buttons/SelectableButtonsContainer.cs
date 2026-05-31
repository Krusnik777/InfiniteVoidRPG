using System;
using System.Collections.Generic;
using InfiniteVoidRPG.Game.Services;
using R3;
using UnityEngine;

namespace UI.Buttons
{
    public enum NavigationDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public class SelectableButtonsContainer : MonoBehaviour, IDisposable
    {
        [field: SerializeField] public bool Interactable { get; private set; } = true;
        [SerializeField] private SelectableButton[] m_buttons;

        private Dictionary<SelectableButton, SelectableButton.Context> _buttonsMap;
        private CompositeDisposable _disposables;
        private CompositeDisposable _inputDisposables;

        private SelectableButton _activeButton;

        public void SetInteractable(bool state)
        {
            Interactable = state;

            if (_activeButton == null) return;

            if (_activeButton != null)
            {
                _buttonsMap[_activeButton].SelectAction(state);
            }
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            _inputDisposables?.Dispose();
        }

        public void Init()
        {
            _disposables?.Dispose();

            _buttonsMap = new();
            _disposables = new();

            for (int i = 0; i < m_buttons.Length; i++)
            {
                var button = m_buttons[i];
                var context = button.BindToContainer(this, m_buttons);
                context.SelectAction(false);
                _buttonsMap.Add(button, context);

                _disposables.Add(button.OnSelect.Subscribe(OnButtonSelect));
            }

            _activeButton = m_buttons[0];
            _buttonsMap[_activeButton].SelectAction(true);
        }

        public void EnableInputs(GameInputService gameInputService)
        {
            _inputDisposables?.Dispose();

            _inputDisposables = new()
            {
                gameInputService.OnUISubmitPressed.Subscribe(_ => PressActiveButton()),
                gameInputService.OnUIMovePressed.Subscribe(input =>
                {
                    var direction = NavigationDirection.Left;

                    if (input.x > 0) direction = NavigationDirection.Right;
                    else if (input.y > 0) direction = NavigationDirection.Up;
                    else if (input.y < 0) direction = NavigationDirection.Down;

                    SelectNextButton(direction);
                })
            };
        }

        public void DisableInputs()
        {
            _inputDisposables?.Dispose();
        }

        private void OnButtonSelect(SelectableButton button)
        {
            if (!Interactable) return;

            if (_activeButton != null)
            {
                _buttonsMap[_activeButton].SelectAction(false);
            }

            _activeButton = button;
            _buttonsMap[_activeButton].SelectAction(true);
        }

        private void SelectNextButton(NavigationDirection direction)
        {
            if (!Interactable) return;

            if (_activeButton == null)
            {
                _activeButton = m_buttons[0];
                _buttonsMap[_activeButton].SelectAction(true);

                return;
            }

            var neighbour = _buttonsMap[_activeButton].ChooseNeighbourAction(direction);

            if (neighbour == null) return;

            OnButtonSelect(neighbour);
        }

        private void PressActiveButton()
        {
            if (!Interactable) return;

            if (_activeButton == null) return;

            _buttonsMap[_activeButton].PressAction();
        }
    }
}
