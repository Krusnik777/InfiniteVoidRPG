using Screen = InfiniteVoidRPG.UI.Screen;

namespace InfiniteVoidRPG.Game
{
    public interface IWindowsFactory
    {
        public T CreateScreen<T>() where T : Screen;
    }
}
