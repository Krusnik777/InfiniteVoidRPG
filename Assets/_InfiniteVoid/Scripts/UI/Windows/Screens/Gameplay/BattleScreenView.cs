using UnityEngine;
using UnityEngine.UI;

namespace InfiniteVoidRPG.UI.Gameplay
{
    public class BattleScreenView : WindowView
    {
        [field: SerializeField] public Button FinishTurnButton { get; private set; }
        [field: SerializeField] public Button FinishBattleButton { get; private set; }
    }
}
