using InfiniteVoidRPG.Game.Services;
using R3;

namespace InfiniteVoidRPG.UI.Gameplay
{
    public class BattleScreen : Screen
    {
        public Observable<string> OnChoseMade => _onChoseMade;

        private BattleScreenView _concreteView => _view as BattleScreenView;

        private GameInputService _gameInputService;

        private Subject<string> _onChoseMade = new();

        private CompositeDisposable _buttonsDisposables;

        public BattleScreen(BattleScreenView view) : base(view) { }

        public void Initialize(GameInputService gameInputService)
        {
            _gameInputService = gameInputService;
            _concreteView.ButtonsContainer.Init();
        }

        public override void Show()
        {
            base.Show();

            SubscribeToButtons();
        }

        public override void Hide()
        {
            base.Hide();

            _buttonsDisposables?.Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();

            _buttonsDisposables?.Dispose();
            _concreteView.ButtonsContainer?.Dispose();
        }

        public void ShowButtons()
        {
            _concreteView.ButtonsContainer.EnableInputs(_gameInputService);
            _concreteView.ButtonsContainer.gameObject.SetActive(true);
        }

        public void HideButtons()
        {
            _concreteView.ButtonsContainer.DisableInputs();
            _concreteView.ButtonsContainer.gameObject.SetActive(false);
        }

        private void SubscribeToButtons()
        {
            _buttonsDisposables = new()
            {
                _concreteView.FinishTurnButton.OnPress.Subscribe(_ => _onChoseMade.OnNext(string.Empty)),
                _concreteView.FinishBattleButton.OnPress.Subscribe(_ => _onChoseMade.OnNext("finish"))
            };
        }
    }
}
