using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class ExpeditionStateMachine : AbstractStateMachine
    {
        public ExpeditionStateMachine(/*other parameters*/)
        {
            _states = new()
            {
              [typeof(ForkPathState)] = new ForkPathState(this/*, other parameters*/), 
              [typeof(PlayEventState)] = new PlayEventState(this/*, other parameters*/), 
              [typeof(BattleState)] = new BattleState(this/*, other parameters*/)
            };
        }
    }
}
