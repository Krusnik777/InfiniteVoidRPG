using DI;
using InfiniteVoidRPG.Game.Gameplay;
using InfiniteVoidRPG.UI.Gameplay;
using InfiniteVoidRPG.Utils;
using StateMachine;

namespace InfiniteVoidRPG.Game.StateMachines.Battle
{
    public class BattleEndState : IEnterableState
    {
        private IStateMachine _stateMachine;
        private IStateMachine _expeditionStateMachine;
        private DIContainer _sceneContainer;

        private System.IDisposable _disposable;

        public BattleEndState(IStateMachine stateMachine, IStateMachine expeditionStateMachine, DIContainer sceneContainer)
        {
            _stateMachine = stateMachine;
            _expeditionStateMachine = expeditionStateMachine;
            _sceneContainer = sceneContainer;
        }

        public void Enter()
        {
            var gameplayUIWindowsProvider = _sceneContainer.Resolve<UIWindowsProvider>();

            var screen = gameplayUIWindowsProvider.GetScreen<BattleScreen>();
            screen.HideButtons();

            _disposable = R3Extensions.DelayedCall(1f, () => _expeditionStateMachine.SetState<ForkPathState>());
        }

        public void Exit()
        {
            _disposable?.Dispose();
        }
    }
}
