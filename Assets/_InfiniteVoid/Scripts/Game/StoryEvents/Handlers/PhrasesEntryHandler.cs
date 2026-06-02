using System;
using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.UI.Common;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    public class PhrasesEntryHandler : StoryEventEntryHandler
    {
        private PhrasesEntry _phrasesEntry;

        private int _lineIndex;

        public PhrasesEntryHandler(PhrasesEntry phrasesEntry, GameInputService gameInputService, StoryEventScreen currentScreen, Action onEnd) : base(gameInputService, currentScreen, onEnd)
        {
            _phrasesEntry = phrasesEntry;
            
            _lineIndex = -1;

            UpdateEntry();
        }

        protected override void UpdateEntry()
        {
            _lineIndex++;
            
            if (_lineIndex >= _phrasesEntry.Phrases.Length)
            {
                _onEnd?.Invoke();
                return;
            }

            var currentLine = _phrasesEntry.Phrases[_lineIndex];

            HandleLine(currentLine);
        }
    }
}
