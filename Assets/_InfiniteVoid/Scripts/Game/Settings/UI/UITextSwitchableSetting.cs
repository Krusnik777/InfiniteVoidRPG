using TMPro;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Settings
{
    public class UITextSwitchableSetting : UISwitchableSetting
    {
        [SerializeField] private TMP_Text m_text;

        protected override void UpdateVisuals()
        {
            m_text.text = _setting.GetNameOfValue();
        }
    }
}
