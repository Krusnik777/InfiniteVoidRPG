using UI.Buttons;
using UnityEngine;

namespace InfiniteVoidRPG.UI.Hub
{
    public class PreparationScreenView : WindowView
    {
        [field: SerializeField] public SelectableButtonsContainer MainButtonsContainer { get; private set; }
        [field: SerializeField] public SelectableButton StartExpeditionButton { get; private set; }
        [field: SerializeField] public SelectableButton PowerUpButton { get; private set; }
        [field: SerializeField] public SelectableButton TalkButton { get; private set; }
        [field: SerializeField] public SelectableButton ExitGameButton { get; private set; }
        [field: SerializeField] public InputButton SettingsButton { get; private set; }
    }
}
