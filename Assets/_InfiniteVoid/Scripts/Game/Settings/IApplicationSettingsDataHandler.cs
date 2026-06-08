using R3;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Data
{
    public interface IApplicationSettingsDataHandler
    {
        public ApplicationSettingsData Data { get; }
        public Observable<ApplicationSettingsData> SaveData();
        public void ResetData();
    }
}
