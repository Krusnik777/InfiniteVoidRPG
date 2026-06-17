using InfiniteVoidRPG.Game.Services;

namespace InfiniteVoidRPG.UI
{
    public class ConfirmWindowInitData : IPopupInitData
    {
        public GameInputService InputService { get; private set; }
        public ConfirmWindowContext Context { get; private set; }

        public ConfirmWindowInitData(GameInputService inputService, ConfirmWindowContext context)
        {
            InputService = inputService;
            Context = context;
        }
    }
    public class ConfirmWindowContext
    {
        public string Message;
        public System.Action OnAgree;
        public System.Action OnDecline;
        public System.Action OnCancel;
    }
}
