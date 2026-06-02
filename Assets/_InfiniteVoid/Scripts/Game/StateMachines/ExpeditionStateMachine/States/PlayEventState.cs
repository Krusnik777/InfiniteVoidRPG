using DI;
using StateMachine;
using InfiniteVoidRPG.Game.Gameplay;
using InfiniteVoidRPG.UI.Common;
using InfiniteVoidRPG.Game.Services;
using R3;

namespace InfiniteVoidRPG.Game.StateMachines
{
    public class PlayEventState : IEnterableState
    {
        private IStateMachine _stateMachine;
        private DIContainer _sceneContainer;

        private GameplayUIController _gameplayUIController;
        private StoryEventsProvider _storyEventsProvider; // temp
        private StoryEventsController _storyEventController;

        private System.IDisposable _disposable;

        public PlayEventState(IStateMachine stateMachine, DIContainer sceneContainer/*, other parameters*/)
        {
            _stateMachine = stateMachine;
            _sceneContainer = sceneContainer;

            _gameplayUIController = _sceneContainer.Resolve<GameplayUIController>();
            _storyEventsProvider = _sceneContainer.Resolve<StoryEventsProvider>(); // temp
            _storyEventController = _sceneContainer.Resolve<StoryEventsController>();
        }

        public void Enter()
        {
            var window = _gameplayUIController.ShowScreen<StoryEventScreen>();
            var storyEvent = _storyEventsProvider.GetRandomStoryEvent(); // temp

            _disposable = _storyEventController.PlayEvent(window, storyEvent).Subscribe(_ => _stateMachine.SetState<ForkPathState>());
        }

        public void Exit()
        {
            _disposable?.Dispose();
        }
    }
}
