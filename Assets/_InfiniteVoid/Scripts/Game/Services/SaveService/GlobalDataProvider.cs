using System;
using Cysharp.Threading.Tasks;
using InfiniteVoidRPG.Game.Data;
using R3;

namespace InfiniteVoidRPG.Game.Services
{
    public class GlobalDataProvider : IDisposable
    {
        private const string _globalDataFileName = "GlobalData";

        private FileDataHandler<GlobalData> _fileDataHandler;

        private GlobalData _data;

        public GlobalDataProvider()
        {
            _fileDataHandler = new(UnityEngine.Application.persistentDataPath, _globalDataFileName, true);
        }

        public void Dispose()
        {
            
        }

        public Observable<GlobalData> SaveData()
        {
            var onEnd = new Subject<GlobalData>();

            _fileDataHandler.SaveAsync(_data, () => onEnd?.OnNext(_data)).Forget();

            return onEnd;
        }

        public async UniTask<GlobalData> LoadData()
        {
            _data = await _fileDataHandler.LoadAsync();

            return _data;
        }
    }
}
