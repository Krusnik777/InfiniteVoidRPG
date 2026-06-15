using System;
using InfiniteVoidRPG.UI.Gameplay;
using UnityEngine;

namespace InfiniteVoidRPG.Game.Gameplay
{
    public class GameplayWindowsFactory : CommonWindowsFactory
    {
        private string _forkPathScreenViewName = "ForkPathScreenView";
        private string _battleScreenViewName = "BattleScreenView";

        public GameplayWindowsFactory(Transform screensHolder, Transform popupsHolder) : base(screensHolder, popupsHolder) { }

        public override T CreateScreen<T>()
        {
            Type t = typeof(T);

            if (t == typeof(UI.Common.StoryEventScreen)) return base.CreateScreen<T>();

            if (t == typeof(ForkPathScreen))
            {
                var prefabPath = GetGameplayScreenPrefabPath(_forkPathScreenViewName);
                var view = InstantiateWindowViewForScreen<ForkPathScreenView>(prefabPath);

                return new ForkPathScreen(view) as T;
            }

            if (t == typeof(BattleScreen))
            {
                var prefabPath = GetGameplayScreenPrefabPath(_battleScreenViewName);
                var view = InstantiateWindowViewForScreen<BattleScreenView>(prefabPath);

                return new BattleScreen(view) as T;
            }

            throw new ArgumentNullException($"Unsupported class - type of: {t}");
        }

        private string GetGameplayScreenPrefabPath(string viewName)
        {
            return $"Prefabs/UI/Gameplay/Screens/{viewName}";
        }

        private string GetGameplayPopupPrefabPath(string viewName)
        {
            return $"Prefabs/UI/Gameplay/Popups/{viewName}";
        }
    }
}
