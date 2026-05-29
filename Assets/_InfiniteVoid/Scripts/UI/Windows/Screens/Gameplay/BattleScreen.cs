using R3;

namespace InfiniteVoidRPG.UI.Gameplay
{
    public class BattleScreen : Screen
    {
        public Observable<string> OnChoseMade => _onChoseMade;

        private BattleScreenView _concreteView => _view as BattleScreenView;

        private Subject<string> _onChoseMade = new();

        public BattleScreen(BattleScreenView view) : base(view) { }

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

        public void ShowButtons()
        {
            _concreteView.FinishTurnButton.gameObject.SetActive(true);
            _concreteView.FinishBattleButton.gameObject.SetActive(true);
        }

        public void HideButtons()
        {
            _concreteView.FinishTurnButton.gameObject.SetActive(false);
            _concreteView.FinishBattleButton.gameObject.SetActive(false);
        }

        private void SubscribeToButtons()
        {
            _concreteView.FinishTurnButton.onClick.AddListener(() => _onChoseMade.OnNext(string.Empty));
            _concreteView.FinishBattleButton.onClick.AddListener(() => _onChoseMade.OnNext("finish"));
        }

        private void UnsubscribeFromButtons()
        {
            _concreteView.FinishTurnButton.onClick.RemoveAllListeners();
            _concreteView.FinishBattleButton.onClick.RemoveAllListeners();
        }
    }
}
