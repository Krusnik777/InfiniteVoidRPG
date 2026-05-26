using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class EnemyTurnState : IEnterableState
    {
        private IStateMachine _stateMachine;

        public EnemyTurnState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
