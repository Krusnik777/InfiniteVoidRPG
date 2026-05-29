using DI;
using InfiniteVoidRPG.Game.Gameplay;
using InfiniteVoidRPG.UI.Gameplay;
using R3;
using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines.Battle
{
    public class PlayerTurnState : IEnterableState
    {
        private IStateMachine _stateMachine;
        private DIContainer _sceneContainer;

        private System.IDisposable _disposable;

        public PlayerTurnState(IStateMachine stateMachine, DIContainer sceneContainer)
        {
            _stateMachine = stateMachine;
            _sceneContainer = sceneContainer;
        }

        public void Enter()
        {
            var gameplayUIController = _sceneContainer.Resolve<GameplayUIController>();

            var screen = gameplayUIController.GetScreen<BattleScreen>();
            screen.ShowButtons();

            _disposable = screen.OnChoseMade.Subscribe(result =>
            {
                if (result == "finish")
                {
                    _stateMachine.SetState<BattleEndState>();
                }
                else
                {
                    _stateMachine.SetState<BattleFlowCheckState>();
                }
            });
        }

        public void Exit()
        {
            _disposable?.Dispose();
        }
    }
}
