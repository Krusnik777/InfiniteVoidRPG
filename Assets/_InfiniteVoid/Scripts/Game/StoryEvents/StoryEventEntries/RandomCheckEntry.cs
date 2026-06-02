using UnityEngine;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    [CreateAssetMenu(fileName = "RandomCheckEntry", menuName = "Scriptable Objects/Story Event Entries/Random Check Entry")]
    public class RandomCheckEntry : StoryEventEntry
    {
        [field: SerializeField] public StatCheckCondition Condition { get; private set; } // TEMP - TO DO something another
        [field: SerializeField] public StoryEventEntry BigSuccessOutcome { get; private set; }
        [field: SerializeField] public StoryEventEntry SuccessOutcome { get; private set; }
        [field: SerializeField] public StoryEventEntry FailureOutcome { get; private set; }
        [field: SerializeField] public StoryEventEntry BigFailureOutcome { get; private set; }

        public override EntryType Type => EntryType.RandomCheck;
    }
}
