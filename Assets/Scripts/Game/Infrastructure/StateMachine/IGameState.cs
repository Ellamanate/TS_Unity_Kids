namespace Game
{
    public interface IGameState : IState
    {
        public void Initialize(GameStateMachine stateMachine);
    }
}