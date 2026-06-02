using InfiniteVoidRPG.Game.StoryEvents;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Services
{
    public class StoryEventsProvider
    {
        private StoryEventsCollection _storyEventsCollection;

        public StoryEventsProvider()
        {
            _storyEventsCollection = Resources.Load<StoryEventsCollection>("Settings/StoryEventsCollection");
        }

        public StoryEventConfig GetRandomStoryEvent()
        {
            var rnd = Random.Range(0, _storyEventsCollection.StoryEvents.Length);

            return _storyEventsCollection.StoryEvents[rnd];
        }
    
    }
}
