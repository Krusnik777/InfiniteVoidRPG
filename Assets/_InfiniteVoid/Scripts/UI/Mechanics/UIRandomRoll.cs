using System;
using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;

namespace InfiniteVoidRPG.UI
{
    public class UIRandomRoll : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_numbersText;
        [SerializeField] private TMP_Text m_resultText;
        [SerializeField] private float m_animationDuration = 2f;
        [SerializeField] private float m_finalTextAppearDuration = 1f;
        [Header("Colors")]
        [SerializeField] private Color m_bigSuccessColor = Color.gold;
        [SerializeField] private Color m_successColor = Color.blue;
        [SerializeField] private Color m_failureColor = Color.orange;
        [SerializeField] private Color m_bigFailureColor = Color.red;

        public Observable<string> PlayAnimation(int finalValue, int resultType, int minValue = 1, int maxValue = 100)
        {
            var onEnd = new Subject<string>();

            m_numbersText.gameObject.SetActive(true);
            m_resultText.gameObject.SetActive(false);

            switch (resultType)
            {
                case 1:
                    m_resultText.text = "Big Success";
                    m_resultText.color = m_bigSuccessColor;
                    break;

                case 2:
                    m_resultText.text = "Success";
                    m_resultText.color = m_successColor;
                    break;

                case 3:
                    m_resultText.text = "Failure";
                    m_resultText.color = m_failureColor;
                    break;

                case 4:
                    m_resultText.text = "Big Failure";
                    m_resultText.color = m_bigFailureColor;
                    break;
            }

            DOTween.Kill(this);

            float progress = 0f;
            DOTween.To(() => progress, x => progress = x, 1f, m_animationDuration)
                .OnUpdate(() =>
                {
                    float t = progress / m_animationDuration;

                    int range = Mathf.RoundToInt(Mathf.Lerp(maxValue - finalValue, 0, t));
                    int min = Math.Max(minValue, finalValue - range);
                    int max = Math.Min(maxValue, finalValue + range);

                    int currentDisplay = UnityEngine.Random.Range(min, max + 1);
                    m_numbersText.text = currentDisplay.ToString();
                })
                .OnComplete(() =>
                {
                    m_numbersText.text = finalValue.ToString();

                    var targetColor = m_resultText.color;
                    var startColor = m_resultText.color;
                    startColor.a = 0;
                    m_resultText.color = startColor;

                    m_resultText.gameObject.SetActive(true);

                    m_resultText.DOColor(targetColor, m_finalTextAppearDuration).SetEase(Ease.InSine).SetLink(gameObject).OnComplete(() => onEnd?.OnNext(""));
                })
                .SetLink(gameObject);

            return onEnd;
        }

        public void Clear()
        {
            DOTween.Kill(this);

            m_numbersText.gameObject.SetActive(false);
            m_resultText.gameObject.SetActive(false);
        }
    }
}
