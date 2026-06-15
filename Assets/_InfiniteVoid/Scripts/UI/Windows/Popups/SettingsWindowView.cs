using InfiniteVoidRPG.Game.Settings;
using UI.Buttons;
using UnityEngine;

namespace InfiniteVoidRPG.UI
{
    public class SettingsWindowView : WindowView
    {
        [field: SerializeField] public UISetting[] Settings { get; private set; }
        [field: SerializeField] public UIButton CloseButton { get; private set; }
    }
}
