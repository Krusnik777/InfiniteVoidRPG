using System;
using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.UI.Common;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    public class ResultEntryHandler : StoryEventEntryHandler
    {
        private ResultEntry _resultEntry;

        private int _lineIndex;
        
        public ResultEntryHandler(ResultEntry resultEntry, GameInputService gameInputService, StoryEventScreen currentScreen, Action onEnd) : base(gameInputService, currentScreen, onEnd)
        {
            _resultEntry = resultEntry;
            
            _lineIndex = -1;

            UpdateEntry();
        }

        protected override void UpdateEntry()
        {
            _lineIndex++;

            if (_lineIndex >= _resultEntry.Phrases.Length)
            {
                // Also results effects

                _onEnd?.Invoke();
                return;
            }

            var currentLine = _resultEntry.Phrases[_lineIndex];

            HandleLine(currentLine);
        }
    }
}
