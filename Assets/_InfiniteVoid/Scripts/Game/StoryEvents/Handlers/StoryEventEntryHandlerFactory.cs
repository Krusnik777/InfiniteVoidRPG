using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.UI.Common;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    public class StoryEventEntryHandlerFactory
    {
        private GameInputService _gameInputService;

        public StoryEventEntryHandlerFactory(GameInputService gameInputService)
        {
            _gameInputService = gameInputService;
        }

        public StoryEventEntryHandler CreateHandler(StoryEventEntry storyEventEntry, StoryEventScreen currentScreen, System.Action onEnd)
        {
            switch (storyEventEntry.Type)
            {
                case StoryEventEntry.EntryType.Phrases: 
                return new PhrasesEntryHandler(storyEventEntry as PhrasesEntry, _gameInputService, currentScreen, onEnd);
                
                case StoryEventEntry.EntryType.Result:
                return new ResultEntryHandler(storyEventEntry as ResultEntry, _gameInputService, currentScreen, onEnd);

                case StoryEventEntry.EntryType.Choice:
                return new ChoiceEntryHandler(storyEventEntry as ChoiceEntry, _gameInputService, currentScreen, onEnd);
            }

            throw new System.ArgumentOutOfRangeException($"Unsupported storyEventEntry: {storyEventEntry.Type}");
        }
    }
}
