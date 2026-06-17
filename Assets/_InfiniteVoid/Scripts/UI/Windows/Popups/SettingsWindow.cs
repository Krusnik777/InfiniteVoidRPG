using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.Game.Settings;
using R3;
using UnityEngine;

namespace InfiniteVoidRPG.UI
{
    public class SettingsWindowExitParameters
    {
        public System.Action AgreeToApplySettingsAction;
        public System.Action DeclineToApplySettingsAction;
    }

    public class SettingsWindow : Popup
    {
        public Subject<SettingsWindowExitParameters> OnNotAppliedSettingsDetected;

        private SettingsWindowView _concreteView => _view as SettingsWindowView;
        
        private UIInputControlledEntity _controlledEntity;
        private UIInputController _uiInputController;

        private UISetting _activeUISetting;
        private int _activeIndex;

        private System.Func<bool> _checkWaitingApplyFunc;
        private System.Action _confirmSaveAndHideAction;
        private System.Action _declineSaveAndHideAction;

        private CompositeDisposable _selectListenerDisposables;
        private System.IDisposable _closeButtonListenerDisposable;

        public SettingsWindow(SettingsWindowView view) : base(view)
        {
            _controlledEntity = new(this)
            {
                OnSubmit = TryToApplyCurrentSetting,
                OnMove = ControlSettings,
                OnCancel = TryToHide
            };

            OnNotAppliedSettingsDetected = new();
        }

        public override void Show()
        {
            _closeButtonListenerDisposable?.Dispose();

            _closeButtonListenerDisposable = _concreteView.CloseButton.OnPress.Subscribe(_ => TryToHide());

            base.Show();
        }

        public override void Hide()
        {
            _closeButtonListenerDisposable?.Dispose();
            _uiInputController.AssignControlledEntity(this, null);

            base.Hide();
        }

        public override void Dispose()
        {
            _selectListenerDisposables?.Dispose();
            _closeButtonListenerDisposable?.Dispose();
            
            base.Dispose();
        }

        public override void Initialize(IPopupInitData initData = null)
        {
            _selectListenerDisposables?.Dispose();
            _selectListenerDisposables = new();

            _uiInputController = initData.InputService.UIInputController;

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

            _confirmSaveAndHideAction += () =>
            {
                data.ApplicationControlService.ApplyAndSaveSettings();
                Hide();
            };
            _declineSaveAndHideAction += () =>
            {
                data.ApplicationControlService.ResetSettingsToAppliedValues();
                Hide();
            };
            _checkWaitingApplyFunc = () => data.ApplicationControlService.IsAnySettingWaitingApply();
        }

        public void SetAsControlled(bool state = true)
        {
            if (!state)
            {
                _uiInputController.AssignControlledEntity(this, null);

                return;
            }

            _uiInputController.AssignControlledEntity(this, _controlledEntity);
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
            if (_checkWaitingApplyFunc())
            {
                OnNotAppliedSettingsDetected?.OnNext(new SettingsWindowExitParameters
                {
                    AgreeToApplySettingsAction = _confirmSaveAndHideAction,
                    DeclineToApplySettingsAction = _declineSaveAndHideAction
                });
            }
            else
            {
                _confirmSaveAndHideAction?.Invoke();
            }
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
