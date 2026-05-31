using System;
using InfiniteVoidRPG.UI;
using InfiniteVoidRPG.UI.Common;
using InfiniteVoidRPG.UI.Gameplay;
using UnityEngine;
using Screen = InfiniteVoidRPG.UI.Screen;

namespace InfiniteVoidRPG.Game.Gameplay
{
    public class GameplayWindowsFactory : IDisposable
    {
        private string _storyEventScreenViewName = "StoryEventScreenView";
        private string _forkPathScreenViewName = "ForkPathScreenView";
        private string _battleScreenViewName = "BattleScreenView";

        private Transform _screensHolder;
        private Transform _popupsHolder;

        public GameplayWindowsFactory(Transform screensHolder, Transform popupsHolder)
        {
            _screensHolder = screensHolder;
            _popupsHolder = popupsHolder;
        }

        public void Dispose() { }

        public T CreateScreen<T>() where T : Screen
        {
            Type t = typeof(T);

            if (t == typeof(StoryEventScreen))
            {
                var prefabPath = GetCommonUIScreenPrefabPath(_storyEventScreenViewName);
                var view = InstantiateWindowView<StoryEventScreenView>(prefabPath);

                return new StoryEventScreen(view) as T;
            }

            if (t == typeof(ForkPathScreen))
            {
                var prefabPath = GetGameplayUIScreenPrefabPath(_forkPathScreenViewName);
                var view = InstantiateWindowView<ForkPathScreenView>(prefabPath);

                return new ForkPathScreen(view) as T;
            }

            if (t == typeof(BattleScreen))
            {
                var prefabPath = GetGameplayUIScreenPrefabPath(_battleScreenViewName);
                var view = InstantiateWindowView<BattleScreenView>(prefabPath);

                return new BattleScreen(view) as T;
            }

            throw new ArgumentNullException($"Unsupported class - type of: {t}");
        }

        private T InstantiateWindowView<T>(string prefabPath) where T : WindowView
        {
            var prefab = Resources.Load<T>(prefabPath);
            var windowView = GameObject.Instantiate(prefab, _screensHolder);

            return windowView;
        }

        private string GetCommonUIScreenPrefabPath(string viewName)
        {
            return $"Prefabs/UI/Common/Screens/{viewName}";
        }

        private string GetGameplayUIScreenPrefabPath(string viewName)
        {
            return $"Prefabs/UI/Gameplay/Screens/{viewName}";
        }
    }
}
