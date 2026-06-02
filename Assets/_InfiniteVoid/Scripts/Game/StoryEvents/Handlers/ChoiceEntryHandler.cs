using System;
using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.UI.Common;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    public class ChoiceEntryHandler : StoryEventEntryHandler
    {
        private ChoiceEntry _choiceEntry;

        public ChoiceEntryHandler(ChoiceEntry choiceEntry, GameInputService gameInputService, StoryEventScreen currentScreen, Action onEnd) : base(gameInputService, currentScreen, onEnd)
        {
            _choiceEntry = choiceEntry;

            UpdateEntry();
        }

        protected override void UpdateEntry()
        {
            _onEnd?.Invoke();
        }
    }
}
