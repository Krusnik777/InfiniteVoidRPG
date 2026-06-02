using UnityEngine;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    [CreateAssetMenu(fileName = "PhrasesEntry", menuName = "Scriptable Objects/Story Event Entries/Phrases Entry")]
    public class PhrasesEntry : StoryEventEntry
    {
        [field: SerializeField] public string[] Phrases { get; private set; }

        public override EntryType Type => EntryType.Phrases;
    }
}
