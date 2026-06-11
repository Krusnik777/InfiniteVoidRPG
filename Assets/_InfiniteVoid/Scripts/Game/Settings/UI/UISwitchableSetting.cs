using R3;
using UI.Buttons;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Settings
{
    public abstract class UISwitchableSetting : UISetting
    {
        [SerializeField] private UIButton m_leftButton;
        [SerializeField] private UIButton m_rightButton;
        [SerializeField] private bool m_applyAtChange = true;
        [SerializeField] private UIButton m_confirmChangesButton;

        private CompositeDisposable _disposables;

        public override void Setup(ISetting setting)
        {
            base.Setup(setting);

            _disposables = new()
            {
                m_rightButton.OnPress.Subscribe(_ => ChangeToNextValue()),
                m_leftButton.OnPress.Subscribe(_ => ChangeToPreviousValue())
            };

            if (m_confirmChangesButton != null) _disposables.Add(m_confirmChangesButton.OnPress.Subscribe(_ => OnPressed()));
        }

        public override void Dispose()
        {
            _disposables?.Dispose();
        }

        public override void OnPressed()
        {
            if (m_confirmChangesButton == null) return;

            if (!m_confirmChangesButton.gameObject.activeSelf) return;

            _setting.Apply();

            m_confirmChangesButton.gameObject.SetActive(false);
        }

        public override void ChangeToNextValue()
        {
            _setting.ToNextValue(m_applyAtChange);
            
            UpdateButtons();
            UpdateVisuals();
        }

        public override void ChangeToPreviousValue()
        {
            _setting.ToPreviousValue(m_applyAtChange);

            UpdateButtons();
            UpdateVisuals();
        }

        private void UpdateButtons()
        {
            m_rightButton.SetInteractable(!_setting.IsMaxValue());
            m_leftButton.SetInteractable(!_setting.IsMinValue());

            if (!m_applyAtChange && m_confirmChangesButton != null) m_confirmChangesButton.gameObject.SetActive(!_setting.IsCurrentValueApplied());
        }

        protected abstract void UpdateVisuals();
    }
}
