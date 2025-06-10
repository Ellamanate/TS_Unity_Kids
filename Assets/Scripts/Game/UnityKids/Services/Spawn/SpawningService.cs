using UnityEngine;
using Utils;
using Zenject;

namespace Game
{
    public class SpawningService
    {
        private readonly ViewsFactory _factory;
        private readonly GameConfig _gameConfig;
        private readonly DraggingProvider _panelDraggingProvider;
        private readonly DraggingProvider _worldDraggingProvider;
        
        public SpawningService(
            ViewsFactory factory,
            GameConfig gameConfig,
            [Inject(Id = StringKeys.PanelDragId)] DraggingProvider panelDraggingProvider,
            [Inject(Id = StringKeys.WorldDragId)] DraggingProvider worldDraggingProvider)
        {
            _factory = factory;
            _gameConfig = gameConfig;
            _panelDraggingProvider = panelDraggingProvider;
            _worldDraggingProvider = worldDraggingProvider;
        }
        
        public void CreateInitialViews()
        {
            foreach (var color in _gameConfig.Colors)
            {
                CreatePanelView(color);
            }
        }

        public CubeView CreatePanelView(Color color)
        {
            var view = _factory.CreateView(color);
            _panelDraggingProvider.AddView(view);
            
            return view;
        }
        
        public CubeView CreateWorldView(CubeView fromView)
        {
            var view = _factory.CreateFrom(fromView);
            _worldDraggingProvider.AddView(view);
            
            return view;
        }
    }
}