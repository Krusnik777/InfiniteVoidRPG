using System.Threading;
using Cysharp.Threading.Tasks;
using InfiniteVoidRPG.Game.Services;
using R3;
using UI.Buttons;

namespace InfiniteVoidRPG.UI.Common
{
    public class StoryEventScreen : Screen
    {
        private const float _lineTypingSpeed = 30f;

        private StoryEventScreenView _concreteView => _view as StoryEventScreenView;

        private CancellationTokenSource _typingLineCTS;

        public StoryEventScreen(StoryEventScreenView view) : base(view) { }

        #region Typing

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

        #endregion

        #region Choices

        public async UniTask<Observable<UIButton>[]> ShowChoices(string[] names, GameInputService gameInputService)
        {
            int length = names.Length;
            var observables = new Observable<UIButton>[length];

            if (length > _concreteView.Buttons.Length) throw new System.IndexOutOfRangeException("Length of choices names more than count of available buttons");

            for (int i = 0; i < _concreteView.Buttons.Length; i++)
            {
                bool active = i < length;

                if (active)
                {
                    _concreteView.Buttons[i].gameObject.SetActive(true);

                    var text = _concreteView.Buttons[i].GetComponentInChildren<TMPro.TMP_Text>();
                    text.text = names[i];

                    observables[i] = _concreteView.Buttons[i].OnPress;
                }
                else
                {
                    _concreteView.Buttons[i].gameObject.SetActive(false);
                }
            }

            _concreteView.ButtonsContainer.gameObject.SetActive(true);

            await UniTask.WaitForFixedUpdate();
            
            _concreteView.ButtonsContainer.Init(false);
            _concreteView.ButtonsContainer.EnableInputs(gameInputService);

            return observables;
        }

        public void HideChoices()
        {
            if (_concreteView.ButtonsContainer.gameObject.activeSelf)
            {
                _concreteView.ButtonsContainer?.Dispose();
                _concreteView.ButtonsContainer.gameObject.SetActive(false);
            }
        }

        #endregion

        #region Random Rolls

        public Observable<string> PlayRollAnimation(int finalValue, int resultType, int minValue = 1, int maxValue = 100) 
            => _concreteView.RandomRoll.PlayAnimation(finalValue, resultType, minValue, maxValue);

        public void HideRoll() => _concreteView.RandomRoll.Clear();

        public Observable<int> StartSpin(UnityEngine.Sprite[] sprites) => _concreteView.SpinSprites.StartSpin(sprites);
        public void StopSpin() => _concreteView.SpinSprites.StopSpin();
        public void HideSpin() => _concreteView.SpinSprites.Clear();

        #endregion

        public override void Dispose()
        {
            base.Dispose();

            StopCurrentTyping(); // just to be safe
        }

        public override void Show()
        {
            if (_concreteView.ButtonsContainer.gameObject.activeSelf) _concreteView.ButtonsContainer.gameObject.SetActive(false);
            _concreteView.RandomRoll.Clear();
            _concreteView.SpinSprites.Clear();

            base.Show();
        }

        public override void Hide()
        {
            base.Hide();

            if (_concreteView.MessagePanel.activeSelf) _concreteView.MessagePanel.SetActive(false);
            HideChoices();
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
