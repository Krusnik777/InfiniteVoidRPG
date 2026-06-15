using System;
using DI;
using InfiniteVoidRPG.Game.Hub;
using InfiniteVoidRPG.Game.Root;
using InfiniteVoidRPG.Game.Services;
using InfiniteVoidRPG.UI;
using InfiniteVoidRPG.UI.Hub;
using R3;
using UnityEngine;

namespace InfiniteVoidRPG.Game.EntryPoints
{
    public class HubEntryPoint : EntryPoint
    {
        [SerializeField] private UISceneRootView m_sceneUIRootPrefab;

        private Subject<string> _onEnd;

        private IDisposable _preparationScreenListenerDisposable;
        private IDisposable _settingsSignalListenerDisposable;
        private IDisposable _settingsWindowClosedListenerDisposable; // temp

        public override Observable<string> Run(DIContainer sceneContainer)
        {
            Debug.Log("ENTRY POINT: Hub");

            _onEnd = new();

            SetupUI(sceneContainer);
            HandleScreens(sceneContainer); // temp

            return _onEnd;
        }

        private void OnDestroy()
        {
            DisposeOfListeners();
        }

        private void ExitGame()
        {
            DisposeOfListeners();

            _onEnd.OnNext("FINISH");
        }

        private void StartExpedition()
        {
            DisposeOfListeners();

            // Create Expedition Data ?

            _onEnd.OnNext(Scenes.GAMEPLAY);
        }

        private void DisposeOfListeners()
        {
            _preparationScreenListenerDisposable?.Dispose();  
            _settingsSignalListenerDisposable?.Dispose(); 
            _settingsWindowClosedListenerDisposable?.Dispose();
        }

        private void SetupUI(DIContainer sceneContainer)
        {
            var uiRoot = sceneContainer.Resolve<UIRootView>();
            var uiSceneRoot = Instantiate(m_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiSceneRoot.gameObject);

            var windowsFactory = new HubWindowsFactory(uiSceneRoot.ScreensTransform, uiSceneRoot.PopupsTransform);
            sceneContainer.RegisterInstance(new UIWindowsProvider(windowsFactory));
            //sceneContainer.RegisterFactory(_ => new UIWindowsProvider(windowsFactory)).AsSingle();
        }

        private void HandleScreens(DIContainer sceneContainer)
        {
            var hubUIWindowsProvider = sceneContainer.Resolve<UIWindowsProvider>();
            var gameInputService = sceneContainer.Resolve<GameInputService>();
            var screen = hubUIWindowsProvider.ShowScreen<PreparationScreen>();
            screen.Initialize(gameInputService);
            screen.SetButtonsContainerActive(true);

            _preparationScreenListenerDisposable = screen.OnMainButtonPressed.Subscribe(result =>
            {
                if (result == "FINISH")
                {
                    ExitGame();
                    return;
                }

                if (result == Scenes.GAMEPLAY)
                {
                    StartExpedition();
                    return;
                }

                if (result == "TALK")
                {
                    // Show Story Event Screen
                    return;
                }

                if (result == "UPGRADE")
                {
                    // Show Upgrade Screen
                    return;
                }
            });

            _settingsSignalListenerDisposable = screen.OnSettingsButtonPressed.Subscribe(_ =>
            {
                screen.SetButtonsContainerActive(false);
                var window = hubUIWindowsProvider.ShowPopup<SettingsWindow>(new SettingsWindowInitData(sceneContainer.Resolve<ApplicationControlService>(), gameInputService));
                _settingsWindowClosedListenerDisposable = window.OnClose.Subscribe(_ =>
                {
                    _settingsWindowClosedListenerDisposable?.Dispose();

                    screen.SetButtonsContainerActive(true);
                });
                // Subscribe to all Popups closed
            });
        }
    }
}
