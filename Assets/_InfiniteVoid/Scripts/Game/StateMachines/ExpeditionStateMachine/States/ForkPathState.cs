using DI;
using StateMachine;
using R3;
using InfiniteVoidRPG.Game.Gameplay;
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
            var gameplayUIController = _sceneContainer.Resolve<GameplayUIController>();

            var screen = gameplayUIController.ShowScreen<ForkPathScreen>();
            screen.Initialize(_sceneContainer.Resolve<GameInputService>());

            _disposable = screen.OnChoseMade.Subscribe(result =>
            {
                if (result == "backward") return;

                if (result == "left") _stateMachine.SetState<PlayEventState>();
                if (result == "right") _stateMachine.SetState<BattleState>();

                if (result == "forward")
                {
                    UnityEngine.Debug.Log("PRESSED FORWARD");
                }
            });
        }

        public void Exit()
        {
            _disposable?.Dispose();
        }
    }
}
