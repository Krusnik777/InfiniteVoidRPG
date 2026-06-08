using System;
using Cysharp.Threading.Tasks;
using InfiniteVoidRPG.Game.Data;
using InfiniteVoidRPG.Game.Settings;
using R3;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Services
{
    public class ApplicationSettingsProvider : IDisposable, IApplicationSettingsDataHandler
    {
        private const string _applicationSettingsDataFileName = "ApplicationSettingsData";

        private FileDataHandler<ApplicationSettingsData> _fileDataHandler;

        private ApplicationSettingsData _data;
        public ApplicationSettingsData Data => _data;

        private ApplicationSettingsConfig _defaultApplicationSettings;
        public ApplicationSettingsConfig DefaultApplicationSettings => _defaultApplicationSettings;

        public ApplicationSettingsProvider()
        {
            _fileDataHandler = new(Application.persistentDataPath, _applicationSettingsDataFileName, false);
            _defaultApplicationSettings = Resources.Load<ApplicationSettingsConfig>("Settings/ApplicationSettingsConfig");
        }

        public void Dispose()
        {

        }

        public Observable<ApplicationSettingsData> SaveData()
        {
            var onEnd = new Subject<ApplicationSettingsData>();

            _fileDataHandler.SaveAsync(_data, () => onEnd?.OnNext(_data)).Forget();

            return onEnd;
        }

        public async UniTask<ApplicationSettingsData> LoadData()
        {
            _data = await _fileDataHandler.LoadAsync();

            await UniTask.WaitForSeconds(1f);

            return _data;
        }

        public async UniTask<ApplicationSettingsData> CreateData()
        {
            ResetData();

            bool saved = false;

            _fileDataHandler.SaveAsync(_data, () => saved = true).Forget();

            await UniTask.WaitUntil(() => saved == true);

            await UniTask.WaitForSeconds(1f);

            return _data;
        }

        public void ResetData()
        {
            _data = new((int)Localization.LocalizationSystem.CurrentLanguage, _defaultApplicationSettings);
        }
    }
}
