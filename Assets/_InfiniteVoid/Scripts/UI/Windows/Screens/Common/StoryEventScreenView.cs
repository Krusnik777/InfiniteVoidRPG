using TMPro;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace InfiniteVoidRPG.UI.Common
{
    public class StoryEventScreenView : WindowView
    {
        [field: SerializeField] public Image BackgroundImage { get; private set; }
        [field: SerializeField] public GameObject MessagePanel { get; private set; }
        [field: SerializeField] public TMP_Text MessageText { get; private set; }
        [field: SerializeField] public SelectableButtonsContainer ButtonsContainer { get; private set; }
        [field: SerializeField] public SelectableButton[] Buttons { get; private set; }
        [field: SerializeField] public UIRandomRoll RandomRoll { get; private set; }
        [field: SerializeField] public UISpinSprites SpinSprites { get; private set; }
    }
}
