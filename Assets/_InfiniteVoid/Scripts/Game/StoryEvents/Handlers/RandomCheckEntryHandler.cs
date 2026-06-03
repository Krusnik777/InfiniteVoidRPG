using System;
using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.UI.Common;
using R3;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    public class RandomCheckEntryHandler : StoryEventEntryHandler
    {
        private RandomCheckEntry _randomCheckEntry;
        private StoryEventEntryHandlerFactory _parentFactory;

        private bool _rollIsFinished;
        private int _result;

        private IDisposable _randomRollListenerDisposable;

        public RandomCheckEntryHandler(RandomCheckEntry randomCheckEntry, StoryEventEntryHandlerFactory parentFactory, GameInputService gameInputService, StoryEventScreen currentScreen, Action onEnd) : base(gameInputService, currentScreen, onEnd)
        {
            _randomCheckEntry = randomCheckEntry;
            _parentFactory = parentFactory;

            _rollIsFinished = false;

            UpdateEntry();
        }

        public override void Dispose()
        {
            base.Dispose();

            _randomRollListenerDisposable?.Dispose();
        }

        protected override void UpdateEntry()
        {
            if (!_rollIsFinished)
            {
                int rnd = UnityEngine.Random.Range(1, 101);

                int minValue = rnd > _randomCheckEntry.Condition.Value ? _randomCheckEntry.Condition.Value : 1;

                _result = UnityEngine.Random.Range(1, 5);

                _randomRollListenerDisposable = _currentScreen.PlayRollAnimation(rnd, _result, minValue).Subscribe(_ =>
                {
                    _randomRollListenerDisposable?.Dispose();

                    _rollIsFinished = true;

                    SubscribeEntryUpdateToSubmitPressed();
                });
            }
            else
            {
                _currentScreen.HideRoll();

                var entry = _result switch
                {
                    1 => _randomCheckEntry.BigSuccessOutcome,
                    2 => _randomCheckEntry.SuccessOutcome,
                    3 => _randomCheckEntry.FailureOutcome,
                    4 => _randomCheckEntry.BigFailureOutcome,
                    _ => throw new NullReferenceException("Result check failure")
                };

                _parentFactory.ReplaceHandler(entry, _currentScreen, _onEnd);
            }
        }
    }
}
