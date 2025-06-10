using System.Collections.Generic;
using Utils;

namespace Game
{
    public class GameStateMachine : StateMachine<IGameState>
    {
        public GameStateMachine(BootstrapState bootstrapState, GameLoopState gameLoopState)
        {
            States = new Dictionary<string, IGameState>
            {
                { nameof(BootstrapState), bootstrapState.InitializeState(this) },
                { nameof(GameLoopState), gameLoopState.InitializeState(this) },
            };
            
            var state = SetState<BootstrapState>();
            state.Enter();
        }
    }
    
    public static partial class CleanCodeExtensions
    {
        public static IGameState InitializeState(this IGameState state, GameStateMachine stateMachine)
        {
            state.Initialize(stateMachine);
            return state;
        }
    }
}