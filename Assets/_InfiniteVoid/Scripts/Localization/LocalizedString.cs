using TMPro;
using UnityEngine;
using R3;

namespace Localization
{
    public class LocalizedString : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_text;
        [SerializeField] private LocalizationTables m_localizationTableKey;
        [SerializeField] private string m_localizationEntryKey;

        private System.IDisposable _disposable;

        private void OnEnable()
        {
            _disposable?.Dispose();

            _disposable = LocalizationSystem.CurrentLanguage.Subscribe(_ => SetText());
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }

        private void SetText()
        {
            m_text.text = LocalizationSystem.GetLocalizedString(m_localizationTableKey.ToString(), m_localizationEntryKey);
        }
    }
}
