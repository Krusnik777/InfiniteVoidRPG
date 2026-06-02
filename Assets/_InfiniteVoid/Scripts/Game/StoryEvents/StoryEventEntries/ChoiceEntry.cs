using UnityEngine;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    [System.Serializable]
    public class Choice
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public StoryEventEntry Consequence { get; private set; }
    }

    [CreateAssetMenu(fileName = "ChoiceEntry", menuName = "Scriptable Objects/Story Event Entries/Choice Entry")]
    public class ChoiceEntry : StoryEventEntry
    {
        [field: SerializeField] public string Phrase { get; private set; }
        [field: SerializeField] public Choice[] Choices { get; private set; }

        public override EntryType Type => EntryType.Choice;
    }
}
