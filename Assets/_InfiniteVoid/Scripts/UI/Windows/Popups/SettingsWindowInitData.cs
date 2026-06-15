using InfiniteVoidRPG.Game.Services;

namespace InfiniteVoidRPG.UI
{
    public class SettingsWindowInitData : IPopupInitData
    {
        public ApplicationControlService ApplicationControlService { get; private set; }
        public GameInputService InputService { get; private set; }

        public SettingsWindowInitData(ApplicationControlService applicationControlService, GameInputService inputService)
        {
            ApplicationControlService = applicationControlService;
            InputService = inputService;
        }
    }
}
