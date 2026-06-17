using TMPro;
using UI.Buttons;
using UnityEngine;

namespace InfiniteVoidRPG.UI
{
    public class ConfirmWindowView : WindowView
    {
        [field: SerializeField] public TMP_Text Message { get; private set; }
        [field: SerializeField] public SelectableButtonsContainer ButtonsContainer { get; private set; }
        [field: SerializeField] public SelectableButton ConfirmButton { get; private set; }
        [field: SerializeField] public SelectableButton DeclineButton { get; private set; }
        [field: SerializeField] public SelectableButton CancelButton { get; private set; }
    }
}
