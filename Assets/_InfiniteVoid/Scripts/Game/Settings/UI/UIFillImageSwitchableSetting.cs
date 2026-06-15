using UnityEngine;
using UnityEngine.UI;

namespace InfiniteVoidRPG.Game.Settings
{
    public class UIFillImageSwitchableSetting : UISwitchableSetting
    {
        [SerializeField] private Image m_fillImage;

        protected override void UpdateVisuals()
        {
            m_fillImage.fillAmount = _setting.GetCurrentValueDifference();
        }
    }
}
