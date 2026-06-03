using System;
using Cysharp.Threading.Tasks;
using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.UI.Common;
using R3;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    public class ChoiceEntryHandler : StoryEventEntryHandler
    {
        private ChoiceEntry _choiceEntry;
        private StoryEventEntryHandlerFactory _parentFactory;

        private bool _isPlayingLine;

        private CompositeDisposable _choiceListenerDisposables;

        public ChoiceEntryHandler(ChoiceEntry choiceEntry, StoryEventEntryHandlerFactory parentFactory, GameInputService gameInputService, StoryEventScreen currentScreen, Action onEnd) : base(gameInputService, currentScreen, onEnd)
        {
            _choiceEntry = choiceEntry;
            _parentFactory = parentFactory;

            _isPlayingLine = false;

            _choiceListenerDisposables = new();

            UpdateEntry();
        }

        public override void Dispose()
        {
            base.Dispose();

            _choiceListenerDisposables?.Dispose();
        }

        protected override void UpdateEntry()
        {
            if (!_isPlayingLine)
            {
                _isPlayingLine = true;

                HandleLine(_choiceEntry.Phrase);
            }
            else
            {
                DoChoices().Forget();
            }
        }

        private async UniTaskVoid DoChoices()
        {
            var choiceNames = new string[_choiceEntry.Choices.Length];

                for (int i = 0; i < choiceNames.Length; i++)
                {
                    choiceNames[i] = _choiceEntry.Choices[i].Name;
                }

                var observables = await _currentScreen.ShowChoices(choiceNames, _gameInputService);

                for (int i = 0; i < observables.Length; i++)
                {
                    int index = i;

                    observables[index].Subscribe(_ =>
                    {
                        _currentScreen.HideChoices();

                        _parentFactory.ReplaceHandler(_choiceEntry.Choices[index].Consequence, _currentScreen, _onEnd);
                    }).AddTo(_choiceListenerDisposables);
                }
        }
    }
}
