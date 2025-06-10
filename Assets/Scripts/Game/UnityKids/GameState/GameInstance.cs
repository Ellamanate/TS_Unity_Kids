using Zenject;

namespace Game
{
    public class GameInstance : IInitializable
    {
        private readonly GameLoopState _gameLoopState;
        private readonly ViewsFactory _factory;

        public GameInstance(GameLoopState gameLoopState, ViewsFactory factory)
        {
            _gameLoopState = gameLoopState;
            _factory = factory;
        }

        public void Initialize()
        {
            _gameLoopState.SetGameInstance(this);
        }

        public void StartGame()
        {
            _factory.CreateInitialViews();
        }
    }
}