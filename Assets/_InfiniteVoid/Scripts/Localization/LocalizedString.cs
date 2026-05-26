using TMPro;
using UnityEngine;
using R3;
using System.Collections.Generic;

namespace Localization
{
    public class LocalizedString : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_text;
        [SerializeField] private LocalizationTableKey m_localizationTableKey;
        [SerializeField] private string m_localizationEntryKey;
        [Header("Arguments")]
        [SerializeField] private bool m_useArguments;
        [SerializeField] private LocalizationIntArgument[] m_localizationIntArguments;
        [SerializeField] private LocalizationStringArgument[] m_localizationStringArguments;

        private System.IDisposable _disposable;

        private void OnEnable()
        {
            _disposable?.Dispose();

            _disposable = LocalizationSystem.OnCurrentLanguageChange.Subscribe(_ => SetText());
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }

        private void SetText()
        {
            if (!m_useArguments)
            {
                m_text.text = LocalizationSystem.GetLocalizedString(m_localizationTableKey, m_localizationEntryKey);
            }
            else
            {
                var arguments = GetLocalizationArguments();

                m_text.text = LocalizationSystem.GetLocalizedString(m_localizationTableKey, m_localizationEntryKey, arguments);
            }
        }

        private LocalizationArgument[] GetLocalizationArguments()
        {
            var list = new List<LocalizationArgument>();

            for (int i = 0; i < m_localizationIntArguments.Length; i++)
            {
                list.Add(m_localizationIntArguments[i].GetLocalizationArgument());
            }

            for (int i = 0; i < m_localizationStringArguments.Length; i++)
            {
                list.Add(m_localizationStringArguments[i].GetLocalizationArgument());
            }

            return list.ToArray();
        }
    }
}
