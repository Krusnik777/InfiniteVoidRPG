using System;
using InfiniteVoidRPG.Game.StoryEvents;
using InfiniteVoidRPG.UI.Common;
using R3;

namespace InfiniteVoidRPG.Game.Services
{
    public class StoryEventsController : IDisposable
    {
        private StoryEventEntryHandlerFactory _storyEventEntryHandlerFactory;
        
        private StoryEventScreen _currentScreen;
        private StoryEventConfig _currentEvent;
        private StoryEventEntryHandler _currentEntryHandler;

        private int _currentEntryIndex;

        private Subject<string> _onStoryEventEnd;

        private IDisposable _handlerReplacedListenerDisposable;

        public StoryEventsController(GameInputService gameInputService)
        {
            _storyEventEntryHandlerFactory = new(gameInputService);

            _handlerReplacedListenerDisposable = _storyEventEntryHandlerFactory.OnHandlerReplaced.Subscribe(OnStoryEventHandlerReplaced);
        }

        public void Dispose()
        {
            _handlerReplacedListenerDisposable?.Dispose();
        }

        public Observable<string> PlayEvent(StoryEventScreen screen, StoryEventConfig storyEvent)
        {
            _onStoryEventEnd = new();

            _currentScreen = screen;
            _currentEvent = storyEvent;
            _currentEntryIndex = -1;

            HandleNextStoryEvent();

            return _onStoryEventEnd;
        }

        private void HandleNextStoryEvent()
        {
            _currentEntryIndex++;

            if (_currentEntryIndex >= _currentEvent.Entries.Length)
            {
                _onStoryEventEnd?.OnNext("finish");
                return;
            }

            _currentEntryHandler = _storyEventEntryHandlerFactory.CreateHandler(_currentEvent.Entries[_currentEntryIndex], _currentScreen, () =>
            {
                _currentEntryHandler?.Dispose();

                HandleNextStoryEvent();
            });
        }

        private void OnStoryEventHandlerReplaced(StoryEventEntryHandler handler)
        {
            _currentEntryHandler?.Dispose();

            _currentEntryHandler = handler;
        }
    }
}
