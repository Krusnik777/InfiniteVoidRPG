using System;
using InfiniteVoidRPG.UI;
using InfiniteVoidRPG.UI.Common;
using InfiniteVoidRPG.UI.Hub;
using UnityEngine;
using Screen = InfiniteVoidRPG.UI.Screen;

namespace InfiniteVoidRPG.Game.Hub
{
    public class HubWindowsFactory : IWindowsFactory, IDisposable
    {
        private string _storyEventScreenViewName = "StoryEventScreenView";
        private string _preparationScreenViewName = "PreparationScreenView";
        private string _upgradeTreeScreenViewName = "UpgradeTreeScreenView";

        private Transform _screensHolder;
        private Transform _popupsHolder;

        public HubWindowsFactory(Transform screensHolder, Transform popupsHolder)
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

            if (t == typeof(PreparationScreen))
            {
                var prefabPath = GetHubUIScreenPrefabPath(_preparationScreenViewName);
                var view = InstantiateWindowView<PreparationScreenView>(prefabPath);

                return new PreparationScreen(view) as T;
            }

            if (t == typeof(UpgradeTreeScreen))
            {
                var prefabPath = GetHubUIScreenPrefabPath(_upgradeTreeScreenViewName);
                var view = InstantiateWindowView<UpgradeTreeScreenView>(prefabPath);

                return new UpgradeTreeScreen(view) as T;
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

        private string GetHubUIScreenPrefabPath(string viewName)
        {
            return $"Prefabs/UI/Hub/Screens/{viewName}";
        }
    }
}
