using Screen = InfiniteVoidRPG.UI.Screen;
using Popup = InfiniteVoidRPG.UI.Popup;

namespace InfiniteVoidRPG.Game
{
    public interface IWindowsFactory
    {
        public T CreateScreen<T>() where T : Screen;
        public T CreatePopup<T>() where T : Popup;
    }
}
