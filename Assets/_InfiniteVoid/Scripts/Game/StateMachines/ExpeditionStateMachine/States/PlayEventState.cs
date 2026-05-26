using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class PlayEventState : IEnterableState
    {
        private IStateMachine _stateMachine;

        public PlayEventState(IStateMachine stateMachine/*, other parameters*/)
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
