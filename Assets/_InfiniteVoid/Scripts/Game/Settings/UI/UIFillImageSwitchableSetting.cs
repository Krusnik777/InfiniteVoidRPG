using UnityEngine;
using UnityEngine.UI;

namespace InfiniteVoidRPG.Game.Settings
{
    public class UIFillImageSwitchableSetting : UISwitchableSetting
    {
        [SerializeField] private Image m_fillImage;

        public override void Setup(ISetting setting)
        {
            base.Setup(setting);

            UpdateVisuals();
        }

        protected override void UpdateVisuals()
        {
            m_fillImage.fillAmount = _setting.GetCurrentValueDifference();
        }
    }
}
