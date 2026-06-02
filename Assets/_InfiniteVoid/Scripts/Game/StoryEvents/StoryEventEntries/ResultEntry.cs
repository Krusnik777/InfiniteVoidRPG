using UnityEngine;

namespace InfiniteVoidRPG.Game.StoryEvents
{
    [System.Serializable]
    public class Result
    {
        [field: SerializeField] public string TypeId { get; private set; } // TEMP - TO DO enums
        [field: SerializeField] public int KeyId { get; private set; }
    }

    [CreateAssetMenu(fileName = "ResultEntry", menuName = "Scriptable Objects/Story Event Entries/Result Entry")]
    public class ResultEntry : StoryEventEntry
    {
        [field: SerializeField] public string[] Phrases { get; private set; }
        [field: SerializeField] public Result Result { get; private set; }

        public override EntryType Type => EntryType.Result;
    }
}
