using TMPro;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Settings
{
    public class UITextSwitchableSetting : UISwitchableSetting
    {
        [SerializeField] private TMP_Text m_text;

        public override void Setup(ISetting setting)
        {
            base.Setup(setting);

            UpdateVisuals();
        }

        protected override void UpdateVisuals()
        {
            m_text.text = _setting.GetNameOfValue();
        }
    }
}
