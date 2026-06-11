using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;
using R3;

namespace InfiniteVoidRPG.Game.Settings
{
    public class UIToggleSetting : UISetting
    {
        [SerializeField] private UIButton m_button;
        [SerializeField] private Image m_toggleImage;

        private System.IDisposable _disposable;

        public override void Setup(ISetting setting)
        {
            if (setting.GetValue() is not bool)
            {
                Debug.LogError("UIToggleSetting: Trying to bind unsupported ISetting");
                return;                
            }

            base.Setup(setting);

            m_toggleImage.enabled = (bool)_setting.GetValue();

            _disposable = m_button.OnPress.Subscribe(_ => OnPressed());
        }

        public override void Dispose()
        {
            _disposable?.Dispose();
        }

        public override void OnPressed()
        {
            bool value = (bool)_setting.GetValue();

            if (value)
            {
                _setting.ToPreviousValue();
            }
            else
            {
                _setting.ToNextValue();
            }

            m_toggleImage.enabled = !value;
        }
    }
}
