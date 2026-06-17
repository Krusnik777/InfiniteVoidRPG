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

        private UIInputControlledEntity _controlledEntity;
        private UIInputController _uiInputController;

        private SelectableButton _activeButton;

        private Dictionary<SelectableButton, SelectableButton.Context> _buttonsMap;
        private CompositeDisposable _disposables;

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
            _uiInputController.AssignControlledEntity(this, null);
            _disposables?.Dispose();
        }

        public void Init(UIInputController uiInputController, bool activateFirstButton = true, bool clearButtons = true, Action cancelAction = null)
        {
            _disposables?.Dispose();

            _buttonsMap = new();
            _disposables = new();

            _uiInputController = uiInputController;

            _controlledEntity = new(this)
            {
                OnSubmit = PressActiveButton,
                OnMove = (input) =>
                {
                    var direction = NavigationDirection.Left;

                    if (input.x > 0) direction = NavigationDirection.Right;
                    else if (input.y > 0) direction = NavigationDirection.Up;
                    else if (input.y < 0) direction = NavigationDirection.Down;

                    SelectNextButton(direction);
                }
            };
            if (cancelAction != null) _controlledEntity.OnCancel = cancelAction;

            if (clearButtons)
            {
                for (int i = 0; i < m_buttons.Length; i++)
                {
                    var button = m_buttons[i];
                    button.ClearNeighbours();
                }
            }

            for (int i = 0; i < m_buttons.Length; i++)
            {
                var button = m_buttons[i];
                var context = button.BindToContainer(this, m_buttons);
                context.SelectAction(false);
                _buttonsMap.Add(button, context);

                _disposables.Add(button.OnSelect.Subscribe(OnButtonSelect));
            }

            if (activateFirstButton)
            {
                _activeButton = m_buttons[0];
                _buttonsMap[_activeButton].SelectAction(true);
            }
            else
            {
                _activeButton = null;
            }
        }

        public void SetAsControlled(bool state = true)
        {
            if (!state)
            {
                _uiInputController.AssignControlledEntity(this, null);

                return;
            }

            _uiInputController.AssignControlledEntity(this, _controlledEntity);
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
