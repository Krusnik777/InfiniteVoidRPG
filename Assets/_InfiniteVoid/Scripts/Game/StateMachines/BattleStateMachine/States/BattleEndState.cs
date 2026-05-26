using StateMachine;

namespace InfiniteVoidRPG
{
    public class BattleEndState : IEnterableState
    {
        private IStateMachine _stateMachine;
        private IStateMachine _expeditionStateMachine;

        public BattleEndState(IStateMachine stateMachine, IStateMachine expeditionStateMachine)
        {
            _stateMachine = stateMachine;
            _expeditionStateMachine = expeditionStateMachine;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
