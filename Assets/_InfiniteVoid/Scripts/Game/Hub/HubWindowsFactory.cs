using System;
using InfiniteVoidRPG.UI.Hub;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Hub
{
    public class HubWindowsFactory : CommonWindowsFactory
    {
        private string _preparationScreenViewName = "PreparationScreenView";
        private string _upgradeTreeScreenViewName = "UpgradeTreeScreenView";

        public HubWindowsFactory(Transform screensHolder, Transform popupsHolder) : base(screensHolder, popupsHolder) { }

        public override T CreateScreen<T>()
        {
            Type t = typeof(T);

            if (t == typeof(UI.Common.StoryEventScreen)) return base.CreateScreen<T>();

            if (t == typeof(PreparationScreen))
            {
                var prefabPath = GetHubScreenPrefabPath(_preparationScreenViewName);
                var view = InstantiateWindowViewForScreen<PreparationScreenView>(prefabPath);

                return new PreparationScreen(view) as T;
            }

            if (t == typeof(UpgradeTreeScreen))
            {
                var prefabPath = GetHubScreenPrefabPath(_upgradeTreeScreenViewName);
                var view = InstantiateWindowViewForScreen<UpgradeTreeScreenView>(prefabPath);

                return new UpgradeTreeScreen(view) as T;
            }

            throw new ArgumentNullException($"Unsupported class - type of: {t}");
        }

        private string GetHubScreenPrefabPath(string viewName)
        {
            return $"Prefabs/UI/Hub/Screens/{viewName}";
        }

        private string GetHubPopupPrefabPath(string viewName)
        {
            return $"Prefabs/UI/Hub/Popups/{viewName}";
        }
    }
}
