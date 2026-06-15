using System;
using InfiniteVoidRPG.UI;
using InfiniteVoidRPG.UI.Common;
using UnityEngine;
using Screen = InfiniteVoidRPG.UI.Screen;

namespace InfiniteVoidRPG.Game
{
    public abstract class CommonWindowsFactory : IWindowsFactory
    {
        private string _storyEventScreenViewName = "StoryEventScreenView";

        private string _settingsWindowViewName = "SettingsWindowView";
        private string _confirmWindowViewName = "ConfirmWindowView";

        private Transform _screensHolder;
        private Transform _popupsHolder;

        public CommonWindowsFactory(Transform screensHolder, Transform popupsHolder)
        {
            _screensHolder = screensHolder;
            _popupsHolder = popupsHolder;
        }

        public virtual T CreateScreen<T>() where T : Screen
        {
            Type t = typeof(T);

            if (t == typeof(StoryEventScreen))
            {
                var prefabPath = GetCommonScreenPrefabPath(_storyEventScreenViewName);
                var view = InstantiateWindowViewForScreen<StoryEventScreenView>(prefabPath);

                return new StoryEventScreen(view) as T;
            }

            throw new ArgumentNullException($"Unsupported class - type of: {t}");
        }

        public virtual T CreatePopup<T>() where T : Popup
        {
            Type t = typeof(T);

            if (t == typeof(SettingsWindow))
            {
                var prefabPath = GetCommonPopupPrefabPath(_settingsWindowViewName);
                var view = InstantiateWindowViewForPopup<SettingsWindowView>(prefabPath);

                return new SettingsWindow(view) as T;
            }

            if (t == typeof(ConfirmWindow))
            {
                var prefabPath = GetCommonPopupPrefabPath(_confirmWindowViewName);
                var view = InstantiateWindowViewForPopup<ConfirmWindowView>(prefabPath);

                return new ConfirmWindow(view) as T;
            }

            throw new ArgumentNullException($"Unsupported class - type of: {t}");
        }

        protected T InstantiateWindowViewForScreen<T>(string prefabPath) where T : WindowView
        {
            var prefab = Resources.Load<T>(prefabPath);
            var windowView = GameObject.Instantiate(prefab, _screensHolder);

            return windowView;
        }

        protected T InstantiateWindowViewForPopup<T>(string prefabPath) where T : WindowView
        {
            var prefab = Resources.Load<T>(prefabPath);
            var windowView = GameObject.Instantiate(prefab, _popupsHolder);

            return windowView;
        }

        private string GetCommonScreenPrefabPath(string viewName)
        {
            return $"Prefabs/UI/Common/Screens/{viewName}";
        }

        private string GetCommonPopupPrefabPath(string viewName)
        {
            return $"Prefabs/UI/Common/Popups/{viewName}";
        }
    }
}
