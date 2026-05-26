using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class BattleFlowCheckState : IEnterableState
    {
        private IStateMachine _stateMachine;

        public BattleFlowCheckState(IStateMachine stateMachine)
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
