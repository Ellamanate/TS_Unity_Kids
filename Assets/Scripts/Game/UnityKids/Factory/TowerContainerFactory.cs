using UnityEngine;

namespace Game
{
    public class TowerContainerFactory
    {
        private readonly Canvas _canvas;
        private readonly RectTransform _gameArea;

        public TowerContainerFactory(Canvas canvas, RectTransform gameArea)
        {
            _canvas = canvas;
            _gameArea = gameArea;
        }

        public RectTransform Create(Vector2 size, Vector3 position)
        {
            var container = new GameObject("Container").AddComponent<RectTransform>();
            container.SetParent(_gameArea, false);
            container.sizeDelta = size;
            container.position = position;

            return container;
        }
    }
}