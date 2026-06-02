using UnityEngine;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    [CreateAssetMenu(fileName = "InputOutcomeEntry", menuName = "Scriptable Objects/Story Event Entries/Input Outcome Entry")]
    public class InputOutcomeEntry : StoryEventEntry
    {
        [field: SerializeField] public string Phrase { get; private set; }
        [field: SerializeField] public StoryEventEntry[] Outcomes { get; private set; }

        public override EntryType Type => EntryType.InputOutcome;
    }
}
