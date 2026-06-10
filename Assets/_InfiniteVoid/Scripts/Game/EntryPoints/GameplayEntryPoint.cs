using DI;
using InfiniteVoidRPG.Game.Gameplay;
using InfiniteVoidRPG.Game.Root;
using InfiniteVoidRPG.Game.StateMachines;
using InfiniteVoidRPG.UI;
using R3;
using UnityEngine;

namespace InfiniteVoidRPG.Game.EntryPoints
{
    public class GameplayEntryPoint : EntryPoint
    {
        [SerializeField] private UISceneRootView m_sceneUIRootPrefab;

        private ExpeditionStateMachine _stateMachine;

        private Subject<string> _onEnd;

        public override Observable<string> Run(DIContainer sceneContainer)
        {
            Debug.Log("ENTRY POINT: Started Gameplay");

            _onEnd = new();

            sceneContainer.RegisterInstance(GameplayStaticTags.HubReturner, new EventInvoker(ReturnToHub));
            
            SetupUI(sceneContainer);

            _stateMachine = new ExpeditionStateMachine(sceneContainer);
            _stateMachine.SetState<ForkPathState>();

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

        private void ReturnToHub()
        {
            DisposeOfListeners();

            _onEnd.OnNext(Scenes.HUB);
        }

        private void DisposeOfListeners()
        {
            _stateMachine?.Dispose();
        }

        private void SetupUI(DIContainer sceneContainer)
        {
            var uiRoot = sceneContainer.Resolve<UIRootView>();
            var uiSceneRoot = Instantiate(m_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiSceneRoot.gameObject);

            var windowsFactory = new GameplayWindowsFactory(uiSceneRoot.ScreensTransform, uiSceneRoot.PopupsTransform);
            sceneContainer.RegisterInstance(new UIWindowsProvider(windowsFactory));
            //sceneContainer.RegisterFactory(_ => new GameplayUIController(windowsFactory)).AsSingle();
        }
    }
}
