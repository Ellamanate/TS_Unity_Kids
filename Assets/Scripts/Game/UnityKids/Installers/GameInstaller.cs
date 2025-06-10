using UnityEngine;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private DragConfig _dragConfig;
        [SerializeField] private AnimationsConfig _animationsConfig;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _canvasTransform;
        [SerializeField] private RectTransform _gameArea;
        [SerializeField] private RectTransform _cubesPanel;
        [SerializeField] private RectTransform _basement;
        [SerializeField] private RectTransform _pit;
        [SerializeField] private Material _maskableMaterial;
        
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
                .WithArguments(_gameConfig, _gameArea, _cubesPanel);
            
            Container
                .Bind<SpawningService>()
                .AsSingle()
                .WithArguments(_gameConfig);
        }
        
        private void BindServices()
        {
            Container
                .Bind<DraggingProvider>()
                .WithId(StringKeys.PanelDragId)
                .FromSubContainerResolve()
                .ByMethod(InstallPanelDraggingProvider)
                .AsCached();

            Container
                .Bind<DraggingProvider>()
                .WithId(StringKeys.WorldDragId)
                .FromSubContainerResolve()
                .ByMethod(InstallWorldDraggingProvider)
                .AsCached();
            
            Container
                .BindInterfacesAndSelfTo<PanelDraggingService>()
                .AsSingle()
                .WithArguments(_dragConfig, _scrollRect)
                .NonLazy();
            
            Container
                .BindInterfacesAndSelfTo<WorldDraggingService>()
                .AsSingle()
                .WithArguments(_gameArea)
                .NonLazy();
            
            Container
                .BindInterfacesAndSelfTo<InteractionService>()
                .AsSingle()
                .WithArguments(_animationsConfig);
            
            Container
                .Bind<InteractionProxy>()
                .AsSingle();
            
            Container
                .BindInterfacesAndSelfTo<OverlappingService>()
                .AsSingle()
                .WithArguments(_gameConfig, _canvas, _basement, _pit);
            
            Container
                .BindInterfacesAndSelfTo<TowerService>()
                .AsSingle()
                .WithArguments(_animationsConfig, _canvas, _gameArea);
            
            Container
                .BindInterfacesAndSelfTo<PitService>()
                .AsSingle()
                .WithArguments(_animationsConfig, _pit, _canvas, _maskableMaterial);
        }
        
        private void InstallPanelDraggingProvider(DiContainer subContainer)
        {
            subContainer
                .BindInterfacesAndSelfTo<DraggingProvider>()
                .AsSingle();
        }

        private void InstallWorldDraggingProvider(DiContainer subContainer)
        {
            subContainer
                .BindInterfacesAndSelfTo<DraggingProvider>()
                .AsSingle();
        }
    }
}