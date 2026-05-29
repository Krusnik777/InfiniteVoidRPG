using UnityEngine;
using UnityEngine.UI;

namespace InfiniteVoidRPG.UI.Gameplay
{
    public class ForkPathScreenView : WindowView
    {
        [field: SerializeField] public Button LeftButton { get; private set; }
        [field: SerializeField] public Button ForwardButton { get; private set; }
        [field: SerializeField] public Button RightButton { get; private set; }
        [field: SerializeField] public Button BackwardButton { get; private set; }
    }
}
