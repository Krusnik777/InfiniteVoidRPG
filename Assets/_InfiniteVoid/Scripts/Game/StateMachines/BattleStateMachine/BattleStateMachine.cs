using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class BattleStateMachine : AbstractStateMachine
    {
        public BattleStateMachine(IStateMachine expeditionStateMachine/*, other parameters*/)
        {
            _states = new()
            {
                [typeof(BattleFlowCheckState)] = new BattleFlowCheckState(this/*, other parameters*/), 
                [typeof(PlayerTurnState)] = new PlayerTurnState(this/*, other parameters*/), 
                [typeof(EnemyTurnState)] = new EnemyTurnState(this/*, other parameters*/),
                [typeof(BattleEndState)] = new BattleEndState(this, expeditionStateMachine/*, other parameters*/)
            };
        }

        public void Cleanup()
        {
            _currentState?.Exit();
        }
    }
}
