using DI;
using InfiniteVoidRPG.Game.Gameplay;
using InfiniteVoidRPG.Game.Root;
using InfiniteVoidRPG.Game.StateMachines;
using InfiniteVoidRPG.UI.Gameplay;
using R3;
using UnityEngine;

namespace InfiniteVoidRPG.Game.EntryPoints
{
    public class GameplayEntryPoint : EntryPoint
    {
        [SerializeField] private UIGameplayRootView m_sceneUIRootPrefab;

        private Subject<string> _onEnd;

        public override Observable<string> Run(DIContainer sceneContainer)
        {
            Debug.Log("ENTRY POINT: Started Gameplay");
            
            SetupUI(sceneContainer);

            var stateMachine = new ExpeditionStateMachine(sceneContainer);
            stateMachine.SetState<ForkPathState>();

            _onEnd = new();

            return _onEnd;
        }

        private void OnDestroy()
        {
            DisposeOfListeners();
        }

        private void FinishGame()
        {
            DisposeOfListeners();

            _onEnd.OnNext("FINISH");
        }

        private void DisposeOfListeners()
        {
            
        }

        private void SetupUI(DIContainer sceneContainer)
        {
            var uiRoot = sceneContainer.Resolve<UIRootView>();
            var uiSceneRoot = Instantiate(m_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiSceneRoot.gameObject);

            var windowsFactory = new GameplayWindowsFactory(uiSceneRoot.ScreensTransform, uiSceneRoot.PopupsTransform);
            sceneContainer.RegisterInstance(new GameplayUIController(windowsFactory));
            //sceneContainer.RegisterFactory(_ => new GameplayUIController(windowsFactory)).AsSingle();
        }
    }
}
