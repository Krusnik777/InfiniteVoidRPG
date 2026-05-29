using DI;
using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines.Battle
{
    public class EnemyTurnState : IEnterableState
    {
        private IStateMachine _stateMachine;
        private DIContainer _sceneContainer;

        public EnemyTurnState(IStateMachine stateMachine, DIContainer sceneContainer)
        {
            _stateMachine = stateMachine;
            _sceneContainer = sceneContainer;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
