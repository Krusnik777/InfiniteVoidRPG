using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace UI.Buttons
{
    public class SelectableButton : UIButton
    {
        public struct Context
        {
            public Action<bool> SelectAction;
            public Action PressAction;
            public Action HoldStartAction;
            public Action HoldStopAction;
            public Func<NavigationDirection, SelectableButton> ChooseNeighbourAction;
        }

        public Subject<SelectableButton> OnSelect { get; private set; } = new();
        public Subject<SelectableButton> OnUnselect { get; private set; } = new();

        protected SelectableButtonsContainer _parentContainer;
        private Dictionary<NavigationDirection, SelectableButton> _neighbourButtons = new()
        {
            { NavigationDirection.Left, null },
            { NavigationDirection.Right, null },
            { NavigationDirection.Up, null },
            { NavigationDirection.Down, null }
        };

        public Context BindToContainer(SelectableButtonsContainer container, SelectableButton[] buttons)
        {
            _parentContainer = container;

            MapNeighbours(buttons);

            return CreateContext();
        }

        public void ClearNeighbours()
        {
            _neighbourButtons = new()
            {
                { NavigationDirection.Left, null },
                { NavigationDirection.Right, null },
                { NavigationDirection.Up, null },
                { NavigationDirection.Down, null }
            };
        }

        protected virtual void OnEnable()
        {
            if (_parentContainer == null) ChangeVisual(false);
        }

        protected override void HandleOnPointerEnter()
        {
            OnSelect.OnNext(this);

            if (_parentContainer == null) base.HandleOnPointerEnter();
        }
        protected override void HandleOnPointerExit()
        {
            OnUnselect.OnNext(this);

            if (_parentContainer == null) base.HandleOnPointerExit();
        }

        protected override void HandleOnPointerClick()
        {
            if (_parentContainer != null && !_parentContainer.Interactable) return;

            base.HandleOnPointerClick();
        }

        protected virtual Context CreateContext()
        {
            return new Context
            {
                SelectAction = ChangeVisual,
                PressAction = HandleOnPointerClick,
                HoldStartAction = () => { },
                HoldStopAction = () => { },
                ChooseNeighbourAction = GetNeighbour
            };
        }

        #region Neighbours Handler

        private void MapNeighbours(SelectableButton[] buttons, bool ignoreMapped = true)
        {
            foreach (NavigationDirection direction in Enum.GetValues(typeof(NavigationDirection)))
            {
                var neighbour = FindNeighbour(buttons, direction, ignoreMapped);

                AddToNeighbours(neighbour, direction);
            }
        }

        private SelectableButton FindNeighbour(SelectableButton[] buttons, NavigationDirection direction, bool ignoreMapped)
        {
            if (!gameObject.activeSelf) return null;

            if (ignoreMapped && _neighbourButtons[direction] != null) return _neighbourButtons[direction];

            var currentPos = transform.localPosition;

            var weight = direction switch
            {
                NavigationDirection.Left => new Vector2(1f, 100f),
                NavigationDirection.Right => new Vector2(1f, 100f),
                NavigationDirection.Up => new Vector2(100f, 1f),
                NavigationDirection.Down => new Vector2(100f, 1f),
                _ => new Vector2(1f, 1f)
            };

            Func<Vector2, Vector2, bool> isInHalfPlane = direction switch
            {
                NavigationDirection.Left => (cand, cur) => cand.x < cur.x - 0.01f,
                NavigationDirection.Right => (cand, cur) => cand.x > cur.x + 0.01f,
                NavigationDirection.Up => (cand, cur) => cand.y > cur.y + 0.01f,
                NavigationDirection.Down => (cand, cur) => cand.y < cur.y - 0.01f,
                _ => null
            };

            SelectableButton finded = null;
            float bestScore = float.MaxValue;

            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];

                if (button == this) continue;

                if (!button.Interactable || !button.gameObject.activeSelf) continue;

                var targetPos = button.transform.localPosition;

                if (!isInHalfPlane(targetPos, currentPos)) continue;

                float deltaX = Mathf.Abs(targetPos.x - currentPos.x);
                float deltaY = Mathf.Abs(targetPos.y - currentPos.y);
                float score = deltaX * weight.x + deltaY * weight.y;

                if (score < bestScore - 0.001f)
                {
                    bestScore = score;
                    finded = button;
                }
            }

            return finded;
        }

        private void AddToNeighbours(SelectableButton neighbour, NavigationDirection direction)
        {
            _neighbourButtons[direction] = neighbour;

            if (neighbour == null) return;

            switch (direction)
            {
                case NavigationDirection.Left: neighbour._neighbourButtons[NavigationDirection.Right] = this; break;
                case NavigationDirection.Right: neighbour._neighbourButtons[NavigationDirection.Left] = this; break;
                case NavigationDirection.Up: neighbour._neighbourButtons[NavigationDirection.Down] = this; break;
                case NavigationDirection.Down: neighbour._neighbourButtons[NavigationDirection.Up] = this; break;
            }
        }

        protected virtual SelectableButton GetNeighbour(NavigationDirection direction)
        {
            return _neighbourButtons[direction];
        }

        #endregion
    }
}
