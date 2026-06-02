using UnityEngine;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    public abstract class StoryEventEntry : ScriptableObject
    {
        public enum EntryType
        {
            Phrases,
            Choice,
            InputOutcome,
            StatCheck,
            RandomCheck,
            Result
        }

        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public Sprite HeaderImage { get; private set; }

        public abstract EntryType Type { get; }
    }
}
