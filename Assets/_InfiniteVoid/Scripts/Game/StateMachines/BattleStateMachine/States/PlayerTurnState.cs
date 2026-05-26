using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class PlayerTurnState : IEnterableState
    {
        private IStateMachine _stateMachine;

        public PlayerTurnState(IStateMachine stateMachine)
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
