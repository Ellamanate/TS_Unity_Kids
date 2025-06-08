using UnityEngine;
using Zenject;

namespace Game
{
    public class GameInstance : IInitializable
    {
        private readonly GameLoopState _gameLoopState;

        public GameInstance(GameLoopState gameLoopState)
        {
            _gameLoopState = gameLoopState;
        }

        public void Initialize()
        {
            _gameLoopState.SetGameInstance(this);
        }

        public void StartGame()
        {
            Debug.Log("Starting Game");
        }
    }
}