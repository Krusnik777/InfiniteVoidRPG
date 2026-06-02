using UnityEngine;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    [CreateAssetMenu(fileName = "StoryEventConfig", menuName = "Scriptable Objects/Story Event Config")]
    public class StoryEventConfig : ScriptableObject
    {
        public enum EventType
        {
            Loot,
            Power,
            Heal,
            Battle,
            Various
        }

        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public EventType Type { get; private set; }
        [field: SerializeField] public Sprite Background { get; private set; }
        [field: SerializeField] public StoryEventEntry[] Entries { get; private set; }
    }
}
