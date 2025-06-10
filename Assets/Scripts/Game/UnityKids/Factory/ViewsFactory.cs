using UnityEngine;

namespace Game
{
    public class ViewsFactory
    {
        private readonly GameConfig _gameConfig;
        private readonly DraggingProvider _draggingProvider;
        private readonly RectTransform _canvasTransform;
        private readonly RectTransform _viewsParent;

        public ViewsFactory(
            GameConfig gameConfig,
            DraggingProvider draggingProvider,
            RectTransform canvasTransform,
            RectTransform viewsParent)
        {
            _gameConfig = gameConfig;
            _draggingProvider = draggingProvider;
            _canvasTransform = canvasTransform;
            _viewsParent = viewsParent;
        }

        public void CreateInitialViews()
        {
            foreach (var color in _gameConfig.Colors)
            {
                CreateDefaultView(color);
            }
        }
        
        public CubeView CreateDefaultView(Color color)
        {
            var cube = Object.Instantiate(_gameConfig.CubeViewPrefab, _viewsParent);
            cube.SetColor(color);
            _draggingProvider.AddView(cube);

            return cube;
        }
        
        public CubeView CreateWorldView(CubeView fromView)
        {
            var cube = Object.Instantiate(
                _gameConfig.CubeViewPrefab,
                fromView.RectTransform.position,
                Quaternion.identity,
                _canvasTransform);
            
            cube.SetColor(fromView.Color);
            _draggingProvider.AddView(cube);
            
            return cube;
        }
        
        public CubeView CreateViewShadow(CubeView fromView)
        {
            var cube = Object.Instantiate(
                _gameConfig.CubeViewPrefab,
                fromView.RectTransform.position,
                Quaternion.identity,
                _canvasTransform);
            
            cube.SetColor(fromView.Color);
            
            return cube;
        }
    }
}