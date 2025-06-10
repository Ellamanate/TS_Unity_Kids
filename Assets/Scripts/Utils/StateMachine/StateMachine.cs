using System.Collections.Generic;

namespace Utils
{
    public abstract class StateMachine<TState> where TState : class, IState
    {
        protected Dictionary<string, TState> States;
        
        public TState CurrentState { get; private set; }
        
        public T MoveToState<T>() where T : class, IEnteringState
        {
            var state = ChangeState<T>();
            state.Enter();
            
            return state;
        }

        public T MoveToState<T, TParam>(TParam value) where T : class, IEnteringState<TParam>
        {
            var state = ChangeState<T>();
            state.Enter(value);
            
            return state;
        }
        
        protected T SetState<T>() where T : class, IState
        {
            string name = typeof(T).Name;
            var state = States[name] as T;
            CurrentState = state as TState;

            return state;
        }
        
        private T ChangeState<T>() where T : class, IState
        {
            ExitState();
            return SetState<T>();
        }

        private void ExitState()
        {
            if (CurrentState is IExitingState exitingState)
            {
                exitingState.Exit();
            }
        }
    }
}