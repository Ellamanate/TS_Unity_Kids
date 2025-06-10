using Zenject;

namespace Game
{
    public class GameInstance : IInitializable
    {
        private readonly GameLoopState _gameLoopState;
        private readonly SpawningService _spawningService;

        public GameInstance(GameLoopState gameLoopState, SpawningService spawningService)
        {
            _gameLoopState = gameLoopState;
            _spawningService = spawningService;
        }

        public void Initialize()
        {
            _gameLoopState.SetGameInstance(this);
        }

        public void StartGame()
        {
            _spawningService.CreateInitialViews();
        }
    }
}