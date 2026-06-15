using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.Game.Settings;
using R3;
using UnityEngine;

namespace InfiniteVoidRPG.UI
{
    public class SettingsWindow : Popup
    {
        private SettingsWindowView _concreteView => _view as SettingsWindowView;

        private UISetting _activeUISetting;
        private int _activeIndex;

        private System.Action _saveAction;

        private CompositeDisposable _inputDisposables;
        private CompositeDisposable _selectListenerDisposables;
        private System.IDisposable _closeButtonListenerDisposable;

        public SettingsWindow(SettingsWindowView view) : base(view) { }

        public override void Show()
        {
            _closeButtonListenerDisposable?.Dispose();

            _closeButtonListenerDisposable = _concreteView.CloseButton.OnPress.Subscribe(_ => TryToHide());

            base.Show();
        }

        public override void Hide()
        {
            _closeButtonListenerDisposable?.Dispose();

            base.Hide();
        }

        public override void Dispose()
        {
            _selectListenerDisposables?.Dispose();
            _inputDisposables?.Dispose();
            _closeButtonListenerDisposable?.Dispose();
            
            base.Dispose();
        }

        public override void Initialize(IPopupInitData initData = null)
        {
            _selectListenerDisposables?.Dispose();
            _selectListenerDisposables = new();

            SetupInputs(initData.InputService);

            if (initData is not SettingsWindowInitData) throw new System.FormatException("Unsupported data for popup - Settings Window");

            var data = initData as SettingsWindowInitData;

            for (int i = 0; i < _concreteView.Settings.Length; i++)
            {
                var uiSetting = _concreteView.Settings[i];

                switch (uiSetting.SettingType)
                {
                    case SettingType.Language:
                        uiSetting.Setup(data.ApplicationControlService.LanguageSetting);
                        break;
                    case SettingType.Resolution:
                        uiSetting.Setup(data.ApplicationControlService.ResolutionSetting);
                        break;
                    case SettingType.ScreenMode:
                        uiSetting.Setup(data.ApplicationControlService.ScreenModeSetting);
                        break;
                    case SettingType.VSync:
                        uiSetting.Setup(data.ApplicationControlService.VSyncSetting);
                        break;
                    case SettingType.SFX:
                        uiSetting.Setup(data.ApplicationControlService.SFXVolumeSetting);
                        break;
                    case SettingType.BGM:
                        uiSetting.Setup(data.ApplicationControlService.BGMVolumeSetting);
                        break;
                }

                uiSetting.SetSelected(false);

                _selectListenerDisposables.Add(uiSetting.OnSelect.Subscribe(OnSettingSelected));
            }

            _activeUISetting = _concreteView.Settings[0];
            _activeIndex = 0;
            _activeUISetting.SetSelected(true);

            _saveAction += () => data.ApplicationControlService.SaveSettings();
        }

        private void SetupInputs(GameInputService gameInputService)
        {
            _inputDisposables?.Dispose();

            _inputDisposables = new()
            {
                gameInputService.OnSelectablesSubmitPressed.Subscribe(_ => TryToApplyCurrentSetting()),
                gameInputService.OnSelectablesMovePressed.Subscribe(input => ControlSettings(input)),
                gameInputService.OnPopupsClosePressed.Subscribe(_ => TryToHide())
            };
        }

        private void TryToApplyCurrentSetting()
        {
            _activeUISetting.OnPressed();
        }

        private void ControlSettings(Vector2 input)
        {
            if (input.y != 0)
            {
                var targetIndex = input.y > 0 ? _activeIndex - 1 : _activeIndex + 1;

                if (targetIndex < 0 || targetIndex > _concreteView.Settings.Length - 1) return;
                
                SelectActiveSetting(targetIndex);

                return;
            }

            if  (input.x != 0)
            {
                bool toRight = input.x > 0;

                if (!toRight) _activeUISetting.ChangeToPreviousValue();
                else _activeUISetting.ChangeToNextValue();
            }
        }

        private void TryToHide()
        {
            _saveAction?.Invoke();

            Hide();
        }

        private void SelectActiveSetting(int targetIndex)
        {
            _activeUISetting.SetSelected(false);

            _activeIndex = targetIndex;
            _activeUISetting = _concreteView.Settings[_activeIndex];
            _activeUISetting.SetSelected(true);
        }

        private void OnSettingSelected(UISetting setting)
        {
            for (int i = 0; i < _concreteView.Settings.Length; i++)
            {
                if (setting == _concreteView.Settings[i])
                {
                    SelectActiveSetting(i);
                    break;
                }
            }
        }
    }
}
