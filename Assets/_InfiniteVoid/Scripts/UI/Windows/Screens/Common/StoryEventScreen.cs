using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace InfiniteVoidRPG.UI.Common
{
    public class StoryEventScreen : Screen
    {
        private const float _lineTypingSpeed = 30f;

        private StoryEventScreenView _concreteView => _view as StoryEventScreenView;

        private CancellationTokenSource _typingLineCTS;

        public StoryEventScreen(StoryEventScreenView view) : base(view) { }

        public Observable<Unit> PlayLine(string text)
        {
            StopCurrentTyping();

            if (!_concreteView.MessagePanel.activeSelf) _concreteView.MessagePanel.SetActive(true);

            _concreteView.MessageText.maxVisibleCharacters = 0;
            _concreteView.MessageText.text = text;

            _typingLineCTS = new();
            var _onEnd = new Subject<Unit>();

            TypeLineAsync(text, _typingLineCTS.Token, _onEnd).Forget();

            return _onEnd;
        }

        public void ShowLineImmediatly(string text)
        {
            StopCurrentTyping();

            if (!_concreteView.MessagePanel.activeSelf) _concreteView.MessagePanel.SetActive(true);

            _concreteView.MessageText.text = text;
            _concreteView.MessageText.maxVisibleCharacters = text.Length;
        }

        public override void Dispose()
        {
            base.Dispose();

            StopCurrentTyping(); // just to be safe
        }

        public override void Hide()
        {
            base.Hide();

            if (_concreteView.MessagePanel.activeSelf) _concreteView.MessagePanel.SetActive(false);
            if (_concreteView.ButtonsContainer.gameObject.activeSelf)
            {
                _concreteView.ButtonsContainer?.Dispose();
                _concreteView.ButtonsContainer.gameObject.SetActive(false);
            }
        }

        private void StopCurrentTyping()
        {
            if (_typingLineCTS != null)
            {
                _typingLineCTS.Cancel();
                _typingLineCTS.Dispose();
                _typingLineCTS = null;
            }
        }

        private async UniTaskVoid TypeLineAsync(string text, CancellationToken token, Subject<Unit> onEnd)
        {
            int totalChars = text.Length;
            float delaySeconds = 1f / _lineTypingSpeed;

            for (int i = 1; i <= totalChars; i++)
            {
                token.ThrowIfCancellationRequested();

                _concreteView.MessageText.maxVisibleCharacters = i;

                await UniTask.Delay(System.TimeSpan.FromSeconds(delaySeconds), cancellationToken: token);
            }

            onEnd.OnNext(Unit.Default);
        }
    }
}
