using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class ForkPathState : IEnterableState
    {
        private IStateMachine _stateMachine;

        public ForkPathState(IStateMachine stateMachine/*, other parameters*/)
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
