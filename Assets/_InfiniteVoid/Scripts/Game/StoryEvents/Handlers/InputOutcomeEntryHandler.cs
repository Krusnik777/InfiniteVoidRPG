using System;
using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.UI.Common;
using UnityEngine;
using R3;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    public class InputOutcomeEntryHandler : StoryEventEntryHandler
    {
        private InputOutcomeEntry _inputOutcomeEntry;
        private StoryEventEntryHandlerFactory _parentFactory;

        private bool _isPlayingLine;
        private int _resultIndex;

        private IDisposable _spinStopListenerDisposable;

        public InputOutcomeEntryHandler(InputOutcomeEntry inputOutcomeEntry, StoryEventEntryHandlerFactory parentFactory, GameInputService gameInputService, StoryEventScreen currentScreen, Action onEnd) : base(gameInputService, currentScreen, onEnd)
        {
            _inputOutcomeEntry = inputOutcomeEntry;
            _parentFactory = parentFactory;

            _isPlayingLine = false;
            _resultIndex = -1;

            UpdateEntry();
        }

        public override void Dispose()
        {
            base.Dispose();

            _spinStopListenerDisposable?.Dispose();
        }

        protected override void UpdateEntry()
        {
            if (!_isPlayingLine)
            {
                _isPlayingLine = true;

                HandleLine(_inputOutcomeEntry.Phrase);
            }
            else
            {
                HandleOutcomes();
            }
        }

        private void HandleOutcomes()
        {
            if (_resultIndex < 0)
            {
                var sprites = new Sprite[_inputOutcomeEntry.Outcomes.Length];

                for (int i = 0; i < sprites.Length; i++) sprites[i] = _inputOutcomeEntry.Outcomes[i].Image;

                _spinStopListenerDisposable = _currentScreen.StartSpin(sprites).Subscribe(index =>
                {
                    _spinStopListenerDisposable?.Dispose();

                    _resultIndex = index;

                    SubscribeEntryUpdateToSubmitPressed();
                });

                SubcribeToSubmitPressed(_currentScreen.StopSpin);
            }
            else
            {
                _parentFactory.ReplaceHandler(_inputOutcomeEntry.Outcomes[_resultIndex].Entry, _currentScreen, _onEnd);
            }
        }
    }
}
