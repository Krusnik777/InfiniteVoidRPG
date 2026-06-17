using R3;

namespace InfiniteVoidRPG.UI
{
    public class ConfirmWindow : Popup
    {
        public override bool IsMultipleInstancesAllowed => true;

        private ConfirmWindowView _concreteView => _view as ConfirmWindowView;

        private ConfirmWindowContext _context;

        private CompositeDisposable _disposables;

        public ConfirmWindow(ConfirmWindowView view) : base(view) { }

        public override void Initialize(IPopupInitData initData = null)
        {
            if (initData is not ConfirmWindowInitData) throw new System.FormatException("Unsupported data for popup - Confirm Window");

            _disposables?.Dispose();

            var data = initData as ConfirmWindowInitData;
            _context = data.Context;

            _concreteView.Message.text = _context.Message;
            _disposables = new()
            {
                _concreteView.ConfirmButton.OnPress.Subscribe(_ => OnAgree()),
                _concreteView.DeclineButton.OnPress.Subscribe(_ => OnDecline()),
                _concreteView.CancelButton.OnPress.Subscribe(_ => OnCancel())
            };

            _concreteView.ButtonsContainer.Init(initData.InputService.UIInputController, true, true, OnCancel);
        }

        public override void Show()
        {
            base.Show();

            _concreteView.ButtonsContainer.SetAsControlled();
        }

        public override void Hide()
        {
            _concreteView.ButtonsContainer.SetAsControlled(false);

            base.Hide();
        }

        public override void Dispose()
        {
            _concreteView.ButtonsContainer?.Dispose();
            _disposables?.Dispose();

            base.Dispose();
        }

        private void OnAgree()
        {
            _context.OnAgree?.Invoke();

            Hide();
        }

        private void OnDecline()
        {
            _context.OnDecline?.Invoke();

            Hide();
        }

        private void OnCancel()
        {
            _context.OnCancel?.Invoke();

            Hide();
        }
    }
}
