using DI;
using InfiniteVoidRPG.Game.Gameplay;
using InfiniteVoidRPG.UI.Gameplay;
using InfiniteVoidRPG.Utils;
using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines.Battle
{
    public class BattleFlowCheckState : IEnterableState
    {
        private IStateMachine _stateMachine;
        private DIContainer _sceneContainer;

        private System.IDisposable _disposable;

        public BattleFlowCheckState(IStateMachine stateMachine, DIContainer sceneContainer)
        {
            _stateMachine = stateMachine;
            _sceneContainer = sceneContainer;
        }

        public void Enter()
        {
            var gameplayUIController = _sceneContainer.Resolve<GameplayUIController>();

            var screen = gameplayUIController.GetScreen<BattleScreen>();
            screen.HideButtons();

            _disposable = R3Extensions.DelayedCall(2f, () => _stateMachine.SetState<PlayerTurnState>());
        }

        public void Exit()
        {
            _disposable?.Dispose();
        }
    }
}
