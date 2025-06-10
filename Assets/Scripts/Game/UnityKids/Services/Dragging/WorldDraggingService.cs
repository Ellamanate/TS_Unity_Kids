using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;
using Zenject;

namespace Game
{
    public class WorldDraggingService : IDisposable
    {
        private readonly InteractionService _interactionService;
        private readonly DraggingProvider _draggingProvider;
        private readonly RectTransform _gameArea;

        private CubeView _currentDraggingView;
        private Vector2 _dragOffset;
        
        public WorldDraggingService(
            InteractionService interactionService,
            [Inject(Id = StringKeys.WorldDragId)] DraggingProvider draggingProvider,
            RectTransform gameArea)
        {
            _interactionService = interactionService;
            _draggingProvider = draggingProvider;
            _gameArea = gameArea;

            _draggingProvider.OnViewDisposedEvent += OnViewDisposed;
            _draggingProvider.OnPointerDownEvent += OnPointerDown;
            _draggingProvider.OnBeginDragEvent += OnBeginDrag;
            _draggingProvider.OnDragEvent += OnDrag;
            _draggingProvider.OnEndDragEvent += OnEndDrag;
        }
        
        public void Dispose()
        {
            _draggingProvider.OnViewDisposedEvent -= OnViewDisposed;
            _draggingProvider.OnPointerDownEvent -= OnPointerDown;
            _draggingProvider.OnBeginDragEvent -= OnBeginDrag;
            _draggingProvider.OnDragEvent -= OnDrag;
            _draggingProvider.OnEndDragEvent -= OnEndDrag;
        }
        
        private void OnViewDisposed(CubeView view)
        {
            if (_currentDraggingView == view)
            {
                _currentDraggingView = null;
            }
        }
        
        private void OnPointerDown(CubeView view, PointerEventData eventData)
        {
            if (_currentDraggingView == null)
            {
                _currentDraggingView = view;
            }
        }
        
        private void OnBeginDrag(CubeView view, PointerEventData eventData)
        {
            if (_currentDraggingView == view)
            {
                _dragOffset = _currentDraggingView.RectTransform.position - (Vector3)eventData.position;
                _interactionService.OnBeginDrag(_currentDraggingView);
                _currentDraggingView.RectTransform.SetParent(_gameArea, true);
            }
        }
        
        private void OnDrag(CubeView view, PointerEventData eventData)
        {
            if (_currentDraggingView == view)
            {
                _interactionService.OnDrag(_currentDraggingView, eventData.position + _dragOffset);
            }
        }
        
        private void OnEndDrag(CubeView view, PointerEventData eventData)
        {
            if (_currentDraggingView == view)
            {
                _interactionService.OnEndDrag(_currentDraggingView);
                _currentDraggingView = null;
            }
        }
    }
}