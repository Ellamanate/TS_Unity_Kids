using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Game
{
    public class InteractionService : IDisposable
    {
        private readonly TowerService _towerService;
        private readonly OverlappingService _overlappingService;
        private readonly PitService _pitService;
        private readonly AnimationsConfig _animationsConfig;
        private readonly CancellationTokenSource _tokenSource;
        
        public InteractionService(
            TowerService towerService,
            OverlappingService overlappingService,
            PitService pitService,
            AnimationsConfig animationsConfig)
        {
            _towerService = towerService;
            _overlappingService = overlappingService;
            _pitService = pitService;
            _animationsConfig = animationsConfig;
            _tokenSource = new CancellationTokenSource();
        }
        
        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        public void OnBeginDrag(CubeView view)
        {            
            if (_towerService.IsInTower(view))
            {
                _towerService.Remove(view);
            }
            
            view.RectTransform.SetSiblingIndex(-1);
        }

        public void OnDrag(CubeView view, Vector3 newPosition)
        {
            view.RectTransform.position = newPosition;
        }
        
        public void OnEndDrag(CubeView view)
        {
            PlaceCube(view);
        }

        private void PlaceCube(CubeView view)
        {
            bool hasCubes = _towerService.HasCubes();

            if (_towerService.CanAddLevel() 
                && (!hasCubes && _overlappingService.IsOverlappingBasement(view.RectTransform)
                || (hasCubes && _overlappingService.IsOverlappingTower(view.RectTransform))))
            {
                _towerService.AddLevel(view);
            }
            else if (_overlappingService.IsOverlappingPit(view.RectTransform))
            {
                _pitService.SetCube(view);
            }
            else
            {
                view.Dispose();
                _ = PlayDestroyAnimation(view, _tokenSource.Token);
            }
        }

        private async UniTaskVoid PlayDestroyAnimation(CubeView view, CancellationToken cancellationToken)
        {
            var tween = view.RectTransform
                .DOScale(Vector3.zero, _animationsConfig.DestroyDuration)
                .SetEase(_animationsConfig.DestroyEase);
            
            await tween.AsyncWaitForKill(cancellationToken);
            
            Object.Destroy(view.gameObject);
        }
    }
}