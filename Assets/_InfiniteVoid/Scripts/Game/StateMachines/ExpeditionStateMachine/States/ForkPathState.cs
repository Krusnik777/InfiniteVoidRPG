using DI;
using StateMachine;
using R3;
using InfiniteVoidRPG.UI.Gameplay;
using InfiniteVoidRPG.Game.Services;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class ForkPathState : IEnterableState
    {
        private IStateMachine _stateMachine;
        private DIContainer _sceneContainer;

        private System.IDisposable _disposable;

        public ForkPathState(IStateMachine stateMachine, DIContainer sceneContainer/*, other parameters*/)
        {
            _stateMachine = stateMachine;
            _sceneContainer = sceneContainer;
        }

        public void Enter()
        {
            var gameplayUIWindowsProvider = _sceneContainer.Resolve<UIWindowsProvider>();

            var screen = gameplayUIWindowsProvider.ShowScreen<ForkPathScreen>();
            screen.Initialize(_sceneContainer.Resolve<GameInputService>());

            _disposable = screen.OnChoseMade.Subscribe(result =>
            {
                if (result == "backward")
                {
                    var invoker = _sceneContainer.Resolve<EventInvoker>(Gameplay.GameplayStaticTags.HubReturner);
                    invoker.InvokeBindedAction();
                    return;
                }

                if (result == "left")
                {
                    _stateMachine.SetState<PlayEventState>();
                    return;
                }
                if (result == "right")
                {
                    _stateMachine.SetState<BattleState>();
                    return;
                }

                if (result == "forward")
                {
                    UnityEngine.Debug.Log("PRESSED FORWARD");
                    return;
                }
            });
        }

        public void Exit()
        {
            _disposable?.Dispose();
        }
    }
}
