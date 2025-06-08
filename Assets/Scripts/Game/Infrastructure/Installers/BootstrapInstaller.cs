using System;
using Zenject;

namespace Game
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStateMachine();
            BindLoadingService();
        }
        
        private void BindStateMachine()
        {
            Container
                .Bind<GameStateMachine>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<BootstrapState>()
                .AsSingle();

            Container
                .Bind(typeof(GameLoopState), typeof(IDisposable))
                .To<GameLoopState>()
                .AsSingle();
        }
        
        private void BindLoadingService()
        {
            Container
                .Bind<ScenesService>()
                .AsSingle();
        }
    }
}
