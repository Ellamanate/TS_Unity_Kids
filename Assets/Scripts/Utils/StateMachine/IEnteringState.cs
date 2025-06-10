namespace Utils
{
    public interface IEnteringState : IState
    {
        public void Enter();
    }
    
    public interface IEnteringState<TParam> : IState
    {
        public void Enter(TParam param);
    }
}