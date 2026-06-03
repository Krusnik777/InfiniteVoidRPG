using UnityEngine;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    [System.Serializable]
    public class InputOutcome
    {
        public StoryEventEntry Entry;
        public Sprite Image;
    }

    [CreateAssetMenu(fileName = "InputOutcomeEntry", menuName = "Scriptable Objects/Story Event Entries/Input Outcome Entry")]
    public class InputOutcomeEntry : StoryEventEntry
    {
        [field: SerializeField] public string Phrase { get; private set; }
        [field: SerializeField] public InputOutcome[] Outcomes { get; private set; }

        public override EntryType Type => EntryType.InputOutcome;
    }
}
