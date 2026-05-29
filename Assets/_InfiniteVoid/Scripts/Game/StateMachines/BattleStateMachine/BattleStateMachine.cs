using DI;
using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines.Battle
{
    public class BattleStateMachine : AbstractStateMachine
    {
        public BattleStateMachine(IStateMachine expeditionStateMachine, DIContainer sceneContainer/*, other parameters*/)
        {
            _states = new()
            {
                [typeof(BattleFlowCheckState)] = new BattleFlowCheckState(this, sceneContainer/*, other parameters*/), 
                [typeof(PlayerTurnState)] = new PlayerTurnState(this, sceneContainer/*, other parameters*/), 
                [typeof(EnemyTurnState)] = new EnemyTurnState(this, sceneContainer/*, other parameters*/),
                [typeof(BattleEndState)] = new BattleEndState(this, expeditionStateMachine, sceneContainer/*, other parameters*/)
            };
        }

        public void Cleanup()
        {
            _currentState?.Exit();
        }
    }
}
