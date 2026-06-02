using System;
using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.UI.Common;
using R3;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    public abstract class StoryEventEntryHandler : IDisposable
    {
        protected GameInputService _gameInputService;
        protected StoryEventScreen _currentScreen;
        protected Action _onEnd;

        private Observable<Unit> _submitPressed;

        private IDisposable _lineTypeListenerDisposable;
        private IDisposable _submitPressListenerDisposable;

        public StoryEventEntryHandler(GameInputService gameInputService, StoryEventScreen currentScreen, Action onEnd)
        {
            _gameInputService = gameInputService;
            _currentScreen = currentScreen;
            _onEnd = onEnd;

            _submitPressed = _gameInputService.OnSelectablesSubmitPressed;
        }

        public virtual void Dispose()
        {
            _lineTypeListenerDisposable?.Dispose();
            _submitPressListenerDisposable?.Dispose();
        }

        protected abstract void UpdateEntry();

        protected void HandleLine(string currentLine)
        {
            _submitPressListenerDisposable = _submitPressed.Subscribe(_ =>
            {
                _currentScreen.ShowLineImmediatly(currentLine);

                SubscribeEntryUpdateToSubmitPressed();
            });

            _lineTypeListenerDisposable = _currentScreen.PlayLine(currentLine).Subscribe(_ => SubscribeEntryUpdateToSubmitPressed());
        }

        protected void SubscribeEntryUpdateToSubmitPressed()
        {
            _lineTypeListenerDisposable?.Dispose();
            _submitPressListenerDisposable?.Dispose();

            _submitPressListenerDisposable = _submitPressed.Subscribe(_ =>
            {
                _submitPressListenerDisposable?.Dispose();

                UpdateEntry();
            });
        }
    }
}
