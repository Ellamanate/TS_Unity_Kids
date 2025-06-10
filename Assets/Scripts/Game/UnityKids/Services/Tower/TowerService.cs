using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Game
{
    public class TowerService : IDisposable
    {
        private class TowerLevel
        {
            public CubeView Cube;
            public RectTransform Container;
            public CancellationTokenSource TokenSource;
            public Vector3 TargetContainerPosition;
        }
        
        private readonly TowerContainerFactory _containerFactory;
        private readonly AnimationsConfig _animationsConfig;
        private readonly Canvas _canvas;
        private readonly RectTransform _gameArea;
        private readonly List<TowerLevel> _levels;
        private readonly float _screenHeight;

        private CancellationTokenSource _tokenSource;
        
        public TowerService(
            TowerContainerFactory containerFactory,
            AnimationsConfig animationsConfig,
            Canvas canvas,
            RectTransform gameArea)
        {
            _containerFactory = containerFactory;
            _animationsConfig = animationsConfig;
            _canvas = canvas;
            _gameArea = gameArea;
            _levels = new List<TowerLevel>();
            _screenHeight = Screen.height;
            _tokenSource = new CancellationTokenSource();
        }
        
        public void Dispose()
        {
            _tokenSource.CancelAndDispose();

            foreach (var level in _levels)
            {
                level.TokenSource.CancelAndDispose();
            }
        }

        public bool TryGetLastCube(out CubeView cube)
        {
            var lastLevel = _levels.LastOrDefault();

            if (lastLevel != null)
            {
                cube = lastLevel.Cube;
                return true;
            }
            
            cube = default;
            return false;
        }
        
        public bool IsInTower(CubeView cube)
        {
            return _levels.Any(x => x.Cube == cube);
        }
        
        public bool HasCubes()
        {
            return _levels.Count > 0;
        }
        
        public bool CanAddLevel()
        {
            if (!HasCubes()) return true;
            
            var lastContainer = _levels.Last().Container;
            var nextPosition = lastContainer.position.y + lastContainer.sizeDelta.y * _canvas.scaleFactor;
            return nextPosition <= _screenHeight;
        }

        public void AddLevel(CubeView cube)
        {
            float cubeHeight = cube.RectTransform.sizeDelta.y * _canvas.scaleFactor;
            var position = HasCubes()
                ? _levels.Last().TargetContainerPosition + new Vector3(0, cubeHeight, 0)
                : cube.RectTransform.position;
            
            var container = _containerFactory.Create(cube.RectTransform.sizeDelta, position);
            cube.RectTransform.SetParent(container);
            
            var level = new TowerLevel
            {
                Cube = cube,
                Container = container,
                TokenSource = new CancellationTokenSource(),
                TargetContainerPosition = container.position,
            };
            
            if (HasCubes())
            {
                float offsetX = cube.RectTransform.sizeDelta.x * _canvas.scaleFactor / 2f;
                offsetX = UnityEngine.Random.Range(-offsetX, offsetX);
                float bounceHeight = cubeHeight * _animationsConfig.BounceHeight;
                var token = level.TokenSource.Token;
                _ = PlayBounceAnimation(cube, bounceHeight, offsetX, token);
            }
            
            _levels.Add(level);
        }
        
        public void Remove(CubeView cube)
        {
            int index = _levels.FindIndex(x => x.Cube == cube);
            
            if (index >= 0)
            {
                var targetLevel = _levels[index];
                targetLevel.TokenSource.CancelAndDispose();
                
                _levels.RemoveAt(index);
                
                _tokenSource = _tokenSource.Refresh();
                var token = _tokenSource.Token;
                
                targetLevel.Cube.RectTransform.SetParent(_gameArea, true);
                Object.Destroy(targetLevel.Container.gameObject);
                
                float cubeHeight = cube.RectTransform.sizeDelta.y * _canvas.scaleFactor;
                _ = PlayFallAnimation(cubeHeight, index, token);
            }
        }
        
        private async UniTaskVoid PlayFallAnimation(
            float cubeHeight,
            int index,
            CancellationToken cancellationToken)
        {
            var sequence = DOTween
                .Sequence()
                .SetEase(_animationsConfig.BounceEase);
                
            for (int i = index; i < _levels.Count; i++)
            {
                var targetPosition = _levels[i].TargetContainerPosition - new Vector3(0, cubeHeight, 0);
                _levels[i].TargetContainerPosition = targetPosition;
                sequence
                    .Join(_levels[i].Container.DOMoveY(targetPosition.y, _animationsConfig.TowerFallDuration))
                    .SetEase(_animationsConfig.TowerFallEase);
            }

            await sequence.AsyncWaitForKill(cancellationToken: cancellationToken);
        }

        private async UniTaskVoid PlayBounceAnimation(
            CubeView cube,
            float height,
            float offsetX,
            CancellationToken cancellationToken)
        {
            var sequence = DOTween.Sequence()
                .Join(cube.RectTransform.DOLocalMoveX(offsetX, _animationsConfig.BounceDuration))
                .Join(DOTween.Sequence()
                    .Join(cube.RectTransform.DOLocalMoveY(height, _animationsConfig.BounceDuration / 2f))
                    .Append(cube.RectTransform.DOLocalMoveY(0, _animationsConfig.BounceDuration / 2f)))
                .SetEase(_animationsConfig.BounceEase);

            await sequence.AsyncWaitForKill(cancellationToken: cancellationToken);
        }
    }
}