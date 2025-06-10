using UnityEngine;

namespace Game
{
    public class ViewsFactory
    {
        private readonly GameConfig _gameConfig;
        private readonly RectTransform _gameArea;
        private readonly RectTransform _viewsParent;

        public ViewsFactory(
            GameConfig gameConfig,
            RectTransform gameArea,
            RectTransform viewsParent)
        {
            _gameConfig = gameConfig;
            _gameArea = gameArea;
            _viewsParent = viewsParent;
        }
        
        public CubeView CreateView(Color color)
        {
            var cube = Object.Instantiate(_gameConfig.CubeViewPrefab, _viewsParent);
            cube.SetColor(color);

            return cube;
        }
        
        public CubeView CreateFrom(CubeView fromView)
        {
            var cube = Object.Instantiate(
                _gameConfig.CubeViewPrefab,
                fromView.RectTransform.position,
                Quaternion.identity,
                _gameArea);
            
            cube.SetColor(fromView.Color);
            
            return cube;
        }
    }
}