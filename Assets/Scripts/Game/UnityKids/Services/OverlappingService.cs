using UnityEngine;
using Utils;

namespace Game
{
    public class OverlappingService
    {
        private readonly GameConfig _gameConfig;
        private readonly TowerService _towerService;
        private readonly RectTransform _basement;

        public OverlappingService(
            GameConfig gameConfig,
            TowerService towerService,
            RectTransform basement)
        {
            _gameConfig = gameConfig;
            _basement = basement;
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
                var cubeHeight = _gameConfig.CubeViewPrefab.RectTransform.sizeDelta.y;
                var offset = new Vector3(0, cubeHeight, 0);
                return Vector2.Distance(view.RectTransform.position + offset, transform.position) < cubeHeight / 2f;
            }

            return false;
        }
    }
}