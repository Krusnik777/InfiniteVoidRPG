using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.UI.Common;
using R3;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    public class StoryEventEntryHandlerFactory
    {
        public Subject<StoryEventEntryHandler> OnHandlerReplaced;

        private GameInputService _gameInputService;

        public StoryEventEntryHandlerFactory(GameInputService gameInputService)
        {
            _gameInputService = gameInputService;

            OnHandlerReplaced = new();
        }

        public StoryEventEntryHandler CreateHandler(StoryEventEntry storyEventEntry, StoryEventScreen currentScreen, System.Action onEnd)
        {
            switch (storyEventEntry.Type)
            {
                case StoryEventEntry.EntryType.Phrases: 
                return new PhrasesEntryHandler(storyEventEntry as PhrasesEntry, _gameInputService, currentScreen, onEnd);

                case StoryEventEntry.EntryType.Choice:
                return new ChoiceEntryHandler(storyEventEntry as ChoiceEntry, this, _gameInputService, currentScreen, onEnd);

                case StoryEventEntry.EntryType.InputOutcome:
                return new InputOutcomeEntryHandler(storyEventEntry as InputOutcomeEntry, this, _gameInputService, currentScreen, onEnd);

                case StoryEventEntry.EntryType.StatCheck:
                return new StatCheckEntryHandler(storyEventEntry as StatCheckEntry, this, _gameInputService, currentScreen, onEnd);

                case StoryEventEntry.EntryType.RandomCheck:
                return new RandomCheckEntryHandler(storyEventEntry as RandomCheckEntry, this, _gameInputService, currentScreen, onEnd);

                case StoryEventEntry.EntryType.Result:
                return new ResultEntryHandler(storyEventEntry as ResultEntry, _gameInputService, currentScreen, onEnd);
            }

            throw new System.ArgumentOutOfRangeException($"Unsupported storyEventEntry: {storyEventEntry.Type}");
        }

        public void ReplaceHandler(StoryEventEntry storyEventEntry, StoryEventScreen currentScreen, System.Action onEnd)
        {
            var replaced = CreateHandler(storyEventEntry, currentScreen, onEnd);

            OnHandlerReplaced?.OnNext(replaced);
        }
    }
}
