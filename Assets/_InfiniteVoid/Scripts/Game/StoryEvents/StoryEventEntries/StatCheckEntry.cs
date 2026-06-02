using UnityEngine;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    [System.Serializable]
    public class StatCheckCondition
    {
        [field: SerializeField] public string StatId { get; private set; } // temp - TO DO enum
        [field: SerializeField] public int Value { get; private set; }
    }

    [CreateAssetMenu(fileName = "StatCheckEntry", menuName = "Scriptable Objects/Story Event Entries/Stat Check Entry")]
    public class StatCheckEntry : StoryEventEntry
    {
        [field: SerializeField] public StatCheckCondition Condition { get; private set; }
        [field: SerializeField] public StoryEventEntry SuccessOutcome { get; private set; }
        [field: SerializeField] public StoryEventEntry FailureOutcome { get; private set; }

        public override EntryType Type => EntryType.StatCheck;
    }
}
