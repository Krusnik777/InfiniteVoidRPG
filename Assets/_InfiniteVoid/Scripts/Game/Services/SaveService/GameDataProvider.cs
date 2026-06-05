using System;
using Cysharp.Threading.Tasks;
using InfiniteVoidRPG.Game.Data;
using R3;

namespace InfiniteVoidRPG.Game.Services
{
    public class GameDataProvider : IDisposable
    {
        private const string _gameDataFileName = "GameData";

        private FileDataHandler<GameData> _fileDataHandler;

        private GameData _data;

        public GameDataProvider()
        {
            _fileDataHandler = new(UnityEngine.Application.persistentDataPath, _gameDataFileName, true);
        }

        public void Dispose()
        {
            
        }

        public Observable<GameData> SaveData()
        {
            var onEnd = new Subject<GameData>();

            _fileDataHandler.SaveAsync(_data, () => onEnd?.OnNext(_data)).Forget();

            return onEnd;
        }

        public async UniTask<GameData> LoadData()
        {
            _data = await _fileDataHandler.LoadAsync();

            return _data;
        }
    }
}
