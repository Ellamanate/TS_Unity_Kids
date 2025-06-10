using Utils;

namespace Game
{
    public class BootstrapState : IGameState, IEnteringState
    {
        private GameStateMachine _stateMachine;
        
        public void Initialize(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _stateMachine.MoveToState<GameLoopState>();
        }
    }
}