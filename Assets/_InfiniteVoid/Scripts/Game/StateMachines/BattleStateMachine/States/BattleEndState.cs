using DI;
using InfiniteVoidRPG.Game.Gameplay;
using InfiniteVoidRPG.UI.Gameplay;
using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines.Battle
{
    public class BattleEndState : IEnterableState
    {
        private IStateMachine _stateMachine;
        private IStateMachine _expeditionStateMachine;
        private DIContainer _sceneContainer;

        public BattleEndState(IStateMachine stateMachine, IStateMachine expeditionStateMachine, DIContainer sceneContainer)
        {
            _stateMachine = stateMachine;
            _expeditionStateMachine = expeditionStateMachine;
            _sceneContainer = sceneContainer;
        }

        public void Enter()
        {
            var gameplayUIController = _sceneContainer.Resolve<GameplayUIController>();

            var screen = gameplayUIController.GetScreen<BattleScreen>();
            screen.HideButtons();

            _expeditionStateMachine.SetState<ForkPathState>();
        }

        public void Exit()
        {
            
        }
    }
}
