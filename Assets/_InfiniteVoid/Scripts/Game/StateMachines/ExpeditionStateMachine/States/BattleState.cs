using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class BattleState : IEnterableState
    {
        private IStateMachine _stateMachine;
        private BattleStateMachine _battleStateMachine;

        public BattleState(IStateMachine stateMachine/*, other parameters*/)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _battleStateMachine = new(_stateMachine);

            _battleStateMachine.SetState<BattleFlowCheckState>();
        }

        public void Exit()
        {
            _battleStateMachine.Cleanup();
        }
    }
}
