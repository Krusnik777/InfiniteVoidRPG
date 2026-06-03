using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace InfiniteVoidRPG.UI
{
    public class UISpinSprites : MonoBehaviour
    {
        [SerializeField] private Image m_targetImage;
        [SerializeField] private float m_spinDuration = 0.05f;

        private Tween _spinTween;

        private Sprite[] _sprites;
        private int _currentIndex;
        private bool _isSpinning;

        private Subject<int> _onSpinEnd;

        public Observable<int> StartSpin(Sprite[] sprites)
        {
            if (_isSpinning) return _onSpinEnd;

            m_targetImage.gameObject.SetActive(true);

            _sprites = sprites;
            _currentIndex = 0;

            _onSpinEnd = new();

            _isSpinning = true;

            PlaySpinSequence();

            return _onSpinEnd;
        }

        public void StopSpin()
        {
            if (!_isSpinning) return;

            _isSpinning = false;
            
            _spinTween?.Kill();

            _onSpinEnd?.OnNext(_currentIndex);
        }

        public void Clear()
        {
            m_targetImage.gameObject.SetActive(false);
        }

        private void PlaySpinSequence()
        {
            if (!_isSpinning) return;

            _currentIndex = (_currentIndex + 1) % _sprites.Length;
            m_targetImage.sprite = _sprites[_currentIndex];

            _spinTween = DOTween.To(() => 0f, x => { }, 1f, m_spinDuration)
                .OnComplete(() => PlaySpinSequence())
                .SetLink(gameObject);
        }
    }
}
