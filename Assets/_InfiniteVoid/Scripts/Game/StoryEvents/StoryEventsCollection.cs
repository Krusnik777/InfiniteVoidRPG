using UnityEngine;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    [CreateAssetMenu(fileName = "StoryEventsCollection", menuName = "Scriptable Objects/Story Events Collection")]
    public class StoryEventsCollection : ScriptableObject
    {
        [field: SerializeField] public StoryEventConfig[] StoryEvents { get; private set; }
    }
}
