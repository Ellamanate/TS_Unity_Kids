using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Game
{
    public class DraggingService : IDisposable
    {
        private readonly InteractionService _selectionService;
        private readonly DraggingProvider _draggingProvider;
        private readonly TowerService _towerService;
        private readonly ViewsFactory _viewsFactory;
        private readonly RectTransform _canvasTransform;
        
        private CubeView _currentDraggingView;
        private CubeView _viewShadow;
        private Vector2 _dragOffset;
        
        public DraggingService(
            DraggingProvider draggingProvider,
            InteractionService selectionService,
            TowerService towerService,
            ViewsFactory viewsFactory,
            RectTransform canvasTransform)
        {
            _draggingProvider = draggingProvider;
            _selectionService = selectionService;
            _towerService = towerService;
            _viewsFactory = viewsFactory;
            _canvasTransform = canvasTransform;

            _draggingProvider.OnBeginDragEvent += OnBeginDrag;
            _draggingProvider.OnDragEvent += OnDrag;
            _draggingProvider.OnEndDragEvent += OnEndDrag;
        }
        
        public void Dispose()
        {
            _draggingProvider.OnBeginDragEvent -= OnBeginDrag;
            _draggingProvider.OnDragEvent -= OnDrag;
            _draggingProvider.OnEndDragEvent -= OnEndDrag;
        }

        private void OnBeginDrag(CubeView view, PointerEventData eventData)
        {
            if (_currentDraggingView == null)
            {
                _currentDraggingView = view;
                _dragOffset = _currentDraggingView.RectTransform.position - (Vector3)eventData.position;
                
                if (_towerService.IsInTower(view))
                {
                    _towerService.Remove(view);
                }
                else
                {
                    _viewShadow = _viewsFactory.CreateViewShadow(_currentDraggingView);
                }
            }
        }
        
        private void OnDrag(CubeView view, PointerEventData eventData)
        {
            if (_currentDraggingView == view)
            {
                var dragTarget = _viewShadow != null ? _viewShadow : _currentDraggingView;
                dragTarget.RectTransform.position = eventData.position + _dragOffset;
            }
        }
        
        private void OnEndDrag(CubeView view, PointerEventData eventData)
        {
            if (_currentDraggingView == view)
            {
                if (_viewShadow != null)
                {
                    _selectionService.OnEndDragShadow(_viewShadow, eventData);
                    Object.Destroy(_viewShadow.gameObject);
                    _viewShadow = null;
                }
                else
                {
                    _selectionService.OnEndDrag(_currentDraggingView, eventData);
                }

                _currentDraggingView = null;
            }
        }
    }
}