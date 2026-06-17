using InfiniteVoidRPG.Game.Services;
using R3;

namespace InfiniteVoidRPG.UI.Hub
{
    public class PreparationScreen : Screen
    {
        public Observable<string> OnMainButtonPressed => _onMainButtonPressed;
        public Observable<Unit> OnSettingsButtonPressed => _onSettingsButtonPressed;

        private PreparationScreenView _concreteView => _view as PreparationScreenView;

        private GameInputService _gameInputService;

        private Subject<string> _onMainButtonPressed = new();
        private Subject<Unit> _onSettingsButtonPressed = new();

        private CompositeDisposable _buttonsDisposables;

        public PreparationScreen(PreparationScreenView view) : base(view) { }

        public void Initialize(GameInputService gameInputService)
        {
            _gameInputService = gameInputService;
            _concreteView.MainButtonsContainer.Init(gameInputService.UIInputController);
            _concreteView.SettingsButton.Init(gameInputService);
        }

        public override void Show()
        {
            base.Show();

            SubscribeToButtons();
        }

        public override void Hide()
        {
            base.Hide();

            SetButtonsContainerActive(false);

            DisposeOfListeners();
        }

        public override void Dispose()
        {
            base.Dispose();

            DisposeOfListeners();
        }

        public void SetButtonsContainerActive(bool state)
        {
            if (state)
            {
                _concreteView.MainButtonsContainer.SetAsControlled();
                _concreteView.SettingsButton.SetInteractable(true);
            }
            else
            {
                _concreteView.MainButtonsContainer.SetAsControlled(false);
                _concreteView.SettingsButton.SetInteractable(false);
            }
        }

        private void SubscribeToButtons()
        {
            _buttonsDisposables?.Dispose();

            _buttonsDisposables = new()
            {
                _concreteView.StartExpeditionButton.OnPress.Subscribe(_ => _onMainButtonPressed.OnNext(Game.Root.Scenes.GAMEPLAY)),
                _concreteView.PowerUpButton.OnPress.Subscribe(_ => _onMainButtonPressed.OnNext("UPGRADE")),
                _concreteView.TalkButton.OnPress.Subscribe(_ => _onMainButtonPressed.OnNext("TALK")),
                _concreteView.ExitGameButton.OnPress.Subscribe(_ => _onMainButtonPressed.OnNext("FINISH")),
                _concreteView.SettingsButton.OnPress.Subscribe(_ => _onSettingsButtonPressed.OnNext(Unit.Default))
            };
        }

        private void DisposeOfListeners()
        {
            _buttonsDisposables?.Dispose();
            _concreteView.MainButtonsContainer?.Dispose();
            _concreteView.SettingsButton.Dispose();
        }
    }
}
