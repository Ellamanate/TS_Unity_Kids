using UnityEngine;
using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _canvasTransform;
        [SerializeField] private RectTransform _cubesPanel;
        [SerializeField] private RectTransform _basement;
        
        public override void InstallBindings()
        {
            BindGameInstance();
            BindCubesFactory();
            BindServices();
        }

        private void BindGameInstance()
        {
            Container
                .BindInterfacesAndSelfTo<GameInstance>()
                .AsSingle();
        }

        private void BindCubesFactory()
        {
            Container
                .Bind<ViewsFactory>()
                .AsSingle()
                .WithArguments(_gameConfig, _canvasTransform, _cubesPanel);
        }
        
        private void BindServices()
        {
            Container
                .BindInterfacesAndSelfTo<DraggingProvider>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<DraggingService>()
                .AsSingle()
                .WithArguments(_canvasTransform);
            
            Container
                .BindInterfacesAndSelfTo<InteractionService>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<OverlappingService>()
                .AsSingle()
                .WithArguments(_gameConfig, _basement);
            
            Container
                .Bind<TowerService>()
                .AsSingle();
        }
    }
}