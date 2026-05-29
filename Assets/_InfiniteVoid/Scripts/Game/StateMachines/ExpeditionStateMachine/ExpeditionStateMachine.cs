using DI;
using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class ExpeditionStateMachine : AbstractStateMachine
    {
        public ExpeditionStateMachine(DIContainer sceneContainer/*, other parameters*/)
        {
            _states = new()
            {
              [typeof(ForkPathState)] = new ForkPathState(this, sceneContainer/*, other parameters*/), 
              [typeof(PlayEventState)] = new PlayEventState(this, sceneContainer/*, other parameters*/), 
              [typeof(BattleState)] = new BattleState(this, sceneContainer/*, other parameters*/)
            };
        }
    }
}
