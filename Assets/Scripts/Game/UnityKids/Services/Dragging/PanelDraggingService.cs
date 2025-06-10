using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;
using Zenject;

namespace Game
{
    public class PanelDraggingService : IDisposable
    {
        private readonly DragConfig _dragConfig;
        private readonly InteractionProxy _interactionService;
        private readonly DraggingProvider _draggingProvider;
        private readonly ScrollRect _scrollRect;
        
        private CubeView _currentDraggingView;
        private Vector2 _dragOffset;
        private Vector2 _pointerStartPos;
        private float _pointerDownTime;
        private bool _isScroll;
        private bool _isElementSelected;
        private bool _isDragging;
        
        public PanelDraggingService(
            DragConfig dragConfig,
            [Inject(Id = StringKeys.PanelDragId)] DraggingProvider draggingProvider,
            InteractionProxy interactionService,
            ScrollRect scrollRect)
        {
            _dragConfig = dragConfig;
            _draggingProvider = draggingProvider;
            _interactionService = interactionService;
            _scrollRect = scrollRect;

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
                ReleaseView();
            }
        }
        
        private void OnPointerDown(CubeView view, PointerEventData eventData)
        {
            if (!_isElementSelected && _currentDraggingView == null)
            {
                _isElementSelected = true;
                _currentDraggingView = view;
                _pointerStartPos = eventData.position;
                _pointerDownTime = Time.time;
                _dragOffset = _currentDraggingView.RectTransform.position - (Vector3)eventData.position;
            }
        }
        
        private void OnBeginDrag(CubeView view, PointerEventData eventData)
        {
            if (_isElementSelected && !_isDragging && _currentDraggingView == view)
            {
                _isDragging = true;
                var delta = eventData.position - _pointerStartPos;
                float deltaTime = Time.time - _pointerDownTime;
                float absX = Mathf.Abs(delta.x);
                float absY = Mathf.Abs(delta.y);
                float speed = delta.magnitude / deltaTime;
                
                if ((absX > absY * _dragConfig.SwipeDirectionFactor || speed > _dragConfig.SwipeSpeedThreshold)
                    && deltaTime < _dragConfig.LongPressTime)
                {
                    _isScroll = true;
                    _scrollRect.enabled = true;
                    _scrollRect.OnBeginDrag(eventData);
                }
                else
                {
                    _isScroll = false;
                    _scrollRect.enabled = false;
                    _interactionService.OnBeginDrag(_currentDraggingView);
                }
            }
        }
        
        private void OnDrag(CubeView view, PointerEventData eventData)
        {
            if (_isElementSelected && _isDragging && _currentDraggingView == view)
            {
                if (_isScroll)
                {
                    _scrollRect.OnDrag(eventData);
                }
                else
                {
                    _interactionService.OnDrag(_currentDraggingView, eventData.position + _dragOffset);
                }
            }
        }
        
        private void OnEndDrag(CubeView view, PointerEventData eventData)
        {
            if (_isElementSelected && _isDragging && _currentDraggingView == view)
            {
                if (_isScroll)
                {
                    _scrollRect.OnEndDrag(eventData);
                }
                else
                {
                    _interactionService.OnEndDrag(_currentDraggingView);
                }

                ReleaseView();
            }
        }

        private void ReleaseView()
        {
            _scrollRect.enabled = true;
            _isScroll = false;
            _isElementSelected = false;
            _isDragging = false;
            _currentDraggingView = null;
        }
    }
}