using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Utils;

namespace Game
{
    public class GameLoopState : IGameState, IEnteringState, IDisposable
    {
        private readonly BootstrapConfig _bootstrapConfig;
        private readonly ScenesService _scenesService;
        private readonly CancellationTokenSource _tokenSource;
        
        private GameStateMachine _stateMachine;
        private GameInstance _gameInstance;
        
        public GameLoopState(BootstrapConfig bootstrapConfig, ScenesService scenesService)
        {
            _bootstrapConfig = bootstrapConfig;
            _scenesService = scenesService;
            _tokenSource = new CancellationTokenSource();
        }

        public void Initialize(GameStateMachine stateMachine)
        {
            
        }
        
        public void Dispose()
        {
            _tokenSource.Dispose();
        }
        
        public void SetGameInstance(GameInstance gameInstance)
        {
            _gameInstance = gameInstance;
        }

        public void Enter()
        {
            _ = EnterGame(_tokenSource.Token);
        }

        private async UniTaskVoid EnterGame(CancellationToken cancellationToken)
        {
            if (_scenesService.CurrentScene != _bootstrapConfig.GameSceneIndex)
            {
                await _scenesService.LoadScene(_bootstrapConfig.GameSceneIndex, _tokenSource.Token);
            }
            
            await UniTask.WaitWhile(() => _gameInstance == null, cancellationToken: cancellationToken);
            
            _gameInstance.StartGame();
        }
    }
}