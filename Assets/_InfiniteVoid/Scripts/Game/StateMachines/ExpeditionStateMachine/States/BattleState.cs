using DI;
using InfiniteVoidRPG.Game.Gameplay;
using InfiniteVoidRPG.Game.StateMachines.Battle;
using InfiniteVoidRPG.UI.Gameplay;
using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class BattleState : IEnterableState
    {
        private IStateMachine _stateMachine;
        private DIContainer _sceneContainer;
        private BattleStateMachine _battleStateMachine;

        public BattleState(IStateMachine stateMachine, DIContainer sceneContainer/*, other parameters*/)
        {
            _stateMachine = stateMachine;
            _sceneContainer = sceneContainer;
        }

        public void Enter()
        {
            var gameplayUIController = _sceneContainer.Resolve<GameplayUIController>();

            gameplayUIController.ShowScreen<BattleScreen>();

            _battleStateMachine = new(_stateMachine, _sceneContainer);

            _battleStateMachine.SetState<BattleFlowCheckState>();
        }

        public void Exit()
        {
            _battleStateMachine.Cleanup();
        }
    }
}
