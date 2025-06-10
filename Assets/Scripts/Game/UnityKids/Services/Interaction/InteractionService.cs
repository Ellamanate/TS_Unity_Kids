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
        private readonly MessageService _messageService;
        private readonly MessagesConfig _messagesConfig;
        private readonly AnimationsConfig _animationsConfig;
        private readonly CancellationTokenSource _tokenSource;
        
        public InteractionService(
            TowerService towerService,
            OverlappingService overlappingService,
            PitService pitService,
            MessageService messageService,
            MessagesConfig messagesConfig,
            AnimationsConfig animationsConfig)
        {
            _towerService = towerService;
            _overlappingService = overlappingService;
            _pitService = pitService;
            _animationsConfig = animationsConfig;
            _messagesConfig = messagesConfig;
            _messageService = messageService;
            _tokenSource = new CancellationTokenSource();
        }
        
        public void Dispose()
        {
            _tokenSource.CancelAndDispose();
        }

        public void OnBeginDrag(CubeView view)
        {            
            if (_towerService.IsInTower(view))
            {
                _towerService.Remove(view);
                _messageService.ShowMessage(_messagesConfig.RemoveTowerCubeKey);
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
            bool overlapTower = _overlappingService.IsOverlappingTower(view.RectTransform);
            bool overlapBasement = _overlappingService.IsOverlappingBasement(view.RectTransform);
            bool canAddLevel = _towerService.CanAddLevel();
            
            if ((!hasCubes && overlapBasement) || (hasCubes && overlapTower && canAddLevel))
            {
                _towerService.AddLevel(view);
                _messageService.ShowMessage(_messagesConfig.AddTowerCubeKey);
            }
            else if (overlapTower && !canAddLevel)
            {
                DestroyCube(view);
                _messageService.ShowMessage(_messagesConfig.TowerFullKey);
            }
            else if (_overlappingService.IsOverlappingPit(view.RectTransform))
            {
                _pitService.SetCube(view);
                _messageService.ShowMessage(_messagesConfig.DropPitKey);
            }
            else
            {
                DestroyCube(view);
                _messageService.ShowMessage(_messagesConfig.DestroyCubeKey);
            }
        }

        private void DestroyCube(CubeView view)
        {
            view.Dispose();
            _ = PlayDestroyAnimation(view, _tokenSource.Token);
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