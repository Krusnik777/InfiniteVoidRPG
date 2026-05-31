using DI;
using StateMachine;
using InfiniteVoidRPG.Game.Gameplay;
using InfiniteVoidRPG.UI.Common;
using InfiniteVoidRPG.Game.Services;
using R3;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class PlayEventState : IEnterableState
    {
        private IStateMachine _stateMachine;
        private DIContainer _sceneContainer;

        private System.IDisposable _disposable;

        public PlayEventState(IStateMachine stateMachine, DIContainer sceneContainer/*, other parameters*/)
        {
            _stateMachine = stateMachine;
            _sceneContainer = sceneContainer;
        }

        public void Enter()
        {
            var gameplayUIController = _sceneContainer.Resolve<GameplayUIController>();
            var window = gameplayUIController.ShowScreen<StoryEventScreen>();
            window.ShowMessage($"Random number : {UnityEngine.Random.Range(0, 99999)}");

            _disposable = _sceneContainer.Resolve<GameInputService>().OnUISubmitPressed.Subscribe(_ => _stateMachine.SetState<ForkPathState>());
        }

        public void Exit()
        {
            _disposable?.Dispose();
        }
    }
}
