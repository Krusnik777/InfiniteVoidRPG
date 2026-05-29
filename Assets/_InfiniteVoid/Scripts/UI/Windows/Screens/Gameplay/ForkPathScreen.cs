using R3;

namespace InfiniteVoidRPG.UI.Gameplay
{
    public class ForkPathScreen : Screen
    {
        public Observable<string> OnChoseMade => _onChoseMade;

        private ForkPathScreenView _concreteView => _view as ForkPathScreenView;
        
        private Subject<string> _onChoseMade = new();

        public ForkPathScreen(ForkPathScreenView view) : base(view) { }

        public override void Show()
        {
            base.Show();

            SubscribeToButtons();
        }

        public override void Hide()
        {
            base.Hide();

            UnsubscribeFromButtons();
        }

        public override void Dispose()
        {
            base.Dispose();

            UnsubscribeFromButtons();
        }

        private void SubscribeToButtons()
        {
            _concreteView.LeftButton.onClick.AddListener(() => _onChoseMade.OnNext("left"));
            _concreteView.RightButton.onClick.AddListener(() => _onChoseMade.OnNext("right"));
            _concreteView.ForwardButton.onClick.AddListener(() => _onChoseMade.OnNext("forward"));
            _concreteView.BackwardButton.onClick.AddListener(() => _onChoseMade.OnNext("backward"));
        }

        private void UnsubscribeFromButtons()
        {
            _concreteView.LeftButton.onClick.RemoveAllListeners();
            _concreteView.RightButton.onClick.RemoveAllListeners();
            _concreteView.ForwardButton.onClick.RemoveAllListeners();
            _concreteView.BackwardButton.onClick.RemoveAllListeners();
        }
    }
}
