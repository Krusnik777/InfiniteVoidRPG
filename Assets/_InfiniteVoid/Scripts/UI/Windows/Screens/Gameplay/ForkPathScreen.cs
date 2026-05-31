using InfiniteVoidRPG.Game.Services;
using R3;

namespace InfiniteVoidRPG.UI.Gameplay
{
    public class ForkPathScreen : Screen
    {
        public Observable<string> OnChoseMade => _onChoseMade;

        private ForkPathScreenView _concreteView => _view as ForkPathScreenView;

        private Subject<string> _onChoseMade = new();

        private CompositeDisposable _buttonsDisposables;

        public ForkPathScreen(ForkPathScreenView view) : base(view) { }

        public void Initialize(GameInputService gameInputService)
        {
            _concreteView.ForwardButton.Init(gameInputService);
            _concreteView.BackwardButton.Init(gameInputService);
            _concreteView.LeftButton.Init(gameInputService);
            _concreteView.RightButton.Init(gameInputService);
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

            _concreteView.ForwardButton.Reset();
            _concreteView.BackwardButton.Reset();
            _concreteView.LeftButton.Reset();
            _concreteView.RightButton.Reset();
        }

        public override void Dispose()
        {
            base.Dispose();

            _buttonsDisposables?.Dispose();

            _concreteView.ForwardButton.Dispose();
            _concreteView.BackwardButton.Dispose();
            _concreteView.LeftButton.Dispose();
            _concreteView.RightButton.Dispose();
        }

        private void SubscribeToButtons()
        {
            _buttonsDisposables = new()
            {
                _concreteView.LeftButton.OnPress.Subscribe(_ => _onChoseMade.OnNext("left")),
                _concreteView.RightButton.OnPress.Subscribe(_ => _onChoseMade.OnNext("right")),
                _concreteView.ForwardButton.OnPress.Subscribe(_ => _onChoseMade.OnNext("forward")),
                _concreteView.BackwardButton.OnPress.Subscribe(_ => _onChoseMade.OnNext("backward"))
            };
        }
    }
}
