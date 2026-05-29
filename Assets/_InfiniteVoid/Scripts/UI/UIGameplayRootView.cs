using UnityEngine;

namespace InfiniteVoidRPG.UI.Gameplay
{
    public class UIGameplayRootView : MonoBehaviour
    {
        [field: SerializeField] public Transform ScreensTransform { get; private set; }
        [field: SerializeField] public Transform PopupsTransform { get; private set; }
    }
}
