using UnityEngine;
using UI.Buttons;

namespace InfiniteVoidRPG.UI.Gameplay
{
    public class ForkPathScreenView : WindowView
    {
        [field: SerializeField] public HoldableInputButton LeftButton { get; private set; }
        [field: SerializeField] public HoldableInputButton ForwardButton { get; private set; }
        [field: SerializeField] public HoldableInputButton RightButton { get; private set; }
        [field: SerializeField] public HoldableInputButton BackwardButton { get; private set; }
    }
}
