using UI.Buttons;
using UnityEngine;

namespace InfiniteVoidRPG.UI.Gameplay
{
    public class BattleScreenView : WindowView
    {
        [field: SerializeField] public SelectableButtonsContainer ButtonsContainer { get; private set; }
        [field: SerializeField] public SelectableButton FinishTurnButton { get; private set; }
        [field: SerializeField] public SelectableButton FinishBattleButton { get; private set; }
    }
}
