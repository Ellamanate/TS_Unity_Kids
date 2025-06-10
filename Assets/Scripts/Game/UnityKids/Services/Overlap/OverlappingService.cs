using UnityEngine;
using Utils;

namespace Game
{
    public class OverlappingService
    {
        private readonly GameConfig _gameConfig;
        private readonly TowerService _towerService;
        private readonly Canvas _canvas;
        private readonly RectTransform _basement;
        private readonly RectTransform _pit;

        public OverlappingService(
            GameConfig gameConfig,
            TowerService towerService,
            Canvas canvas,
            RectTransform basement,
            RectTransform pit)
        {
            _gameConfig = gameConfig;
            _basement = basement;
            _pit = pit;
            _canvas = canvas;
            _towerService = towerService;
        }

        public bool IsOverlappingBasement(RectTransform transform)
        {
            return transform.IsFullyInside(_basement);
        }
        
        public bool IsOverlappingTower(RectTransform transform)
        {
            if (_towerService.TryGetLastCube(out var view))
            {
                var cubeHeight = _gameConfig.CubeViewPrefab.RectTransform.sizeDelta.y * _canvas.scaleFactor;
                var offset = new Vector3(0, cubeHeight, 0);
                return Vector2.Distance(view.RectTransform.position + offset, transform.position) < cubeHeight / 2f;
            }

            return false;
        }
        
        public bool IsOverlappingPit(RectTransform transform)
        {
            return transform.IsInsideOval(_pit, _canvas);
        }
    }
}