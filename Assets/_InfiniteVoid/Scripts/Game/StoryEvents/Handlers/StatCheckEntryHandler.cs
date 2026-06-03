using System;
using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.UI.Common;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    public class StatCheckEntryHandler : StoryEventEntryHandler
    {
        private StatCheckEntry _statCheckEntry;
        private StoryEventEntryHandlerFactory _parentFactory;

        public StatCheckEntryHandler(StatCheckEntry statCheckEntry, StoryEventEntryHandlerFactory parentFactory, GameInputService gameInputService, StoryEventScreen currentScreen, Action onEnd) : base(gameInputService, currentScreen, onEnd)
        {
            _statCheckEntry = statCheckEntry;
            _parentFactory = parentFactory;

            UpdateEntry();
        }

        protected override void UpdateEntry()
        {
            var rnd = UnityEngine.Random.Range(0, 101); // temp

            if (rnd >= _statCheckEntry.Condition.Value)
            {
                _parentFactory.ReplaceHandler(_statCheckEntry.SuccessOutcome, _currentScreen, _onEnd);
            }
            else
            {
                _parentFactory.ReplaceHandler(_statCheckEntry.FailureOutcome, _currentScreen, _onEnd);
            }
        }
    }
}
