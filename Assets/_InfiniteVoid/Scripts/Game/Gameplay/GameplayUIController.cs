using System;
using System.Collections.Generic;
using InfiniteVoidRPG.UI;

namespace InfiniteVoidRPG.Game.Gameplay
{
    public class GameplayUIController : IDisposable
    {
        private GameplayWindowsFactory _windowsFactory;

        private Dictionary<Type, Screen> _createdScreensMap;
        private Screen _activeScreen;

        public GameplayUIController(GameplayWindowsFactory windowsFactory)
        {
            _windowsFactory = windowsFactory;

            _createdScreensMap = new();
        }

        public T GetScreen<T>() where T : Screen
        {
            if (_activeScreen is T) return _activeScreen as T;

            _activeScreen?.Hide();

            if (!_createdScreensMap.ContainsKey(typeof(T)))
            {
                var newScreen = _windowsFactory.CreateScreen<T>();
                _createdScreensMap.Add(typeof(T), newScreen);
            }

            var screen = _createdScreensMap[typeof(T)] as T;
            _activeScreen = screen;

            return screen;
        }

        public T ShowScreen<T>() where T : Screen
        {
            var screen = GetScreen<T>();

            screen.Show();

            return screen;
        }

        public void Dispose()
        {
            foreach (var screen in _createdScreensMap)
            {
                screen.Value?.Dispose();
            }
        }
    }
}
