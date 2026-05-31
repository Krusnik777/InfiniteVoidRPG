using UnityEngine;
using UI.Buttons;

namespace InfiniteVoidRPG.UI.Gameplay
{
    public class ForkPathScreenView : WindowView
    {
        [field: SerializeField] public InputButton LeftButton { get; private set; }
        [field: SerializeField] public InputButton ForwardButton { get; private set; }
        [field: SerializeField] public InputButton RightButton { get; private set; }
        [field: SerializeField] public InputButton BackwardButton { get; private set; }
    }
}
