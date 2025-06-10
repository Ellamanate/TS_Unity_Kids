using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class DraggingProvider : IDisposable
    {
        public event Action<CubeView> OnViewDisposedEvent;
        public event Action<CubeView, PointerEventData> OnPointerDownEvent;
        public event Action<CubeView, PointerEventData> OnBeginDragEvent;
        public event Action<CubeView, PointerEventData> OnDragEvent;
        public event Action<CubeView, PointerEventData> OnEndDragEvent;
        
        private readonly List<CubeView> _createdViews;
        
        public DraggingProvider()
        {
            _createdViews = new List<CubeView>();
        }
        
        public void Dispose()
        {
            foreach (var view in _createdViews)
            {
                view.OnDisposed -= OnViewDisposed;
                view.OnPointerDownEvent -= OnPointerDown;
                view.OnBeginDragEvent -= OnBeginDrag;
                view.OnDragEvent -= OnDrag;
                view.OnEndDragEvent -= OnEndDrag;
            }
        }
        
        public void AddView(CubeView view)
        {
            if (_createdViews.Contains(view))
            {
                Debug.LogError("Something went wrong, view is already added");
                return;
            }
            
            view.OnDisposed += OnViewDisposed;
            view.OnPointerDownEvent += OnPointerDown;
            view.OnBeginDragEvent += OnBeginDrag;
            view.OnDragEvent += OnDrag;
            view.OnEndDragEvent += OnEndDrag;
            _createdViews.Add(view);
        }

        public void RemoveView(CubeView view)
        {
            view.OnDisposed -= OnViewDisposed;
            view.OnPointerDownEvent -= OnPointerDown;
            view.OnBeginDragEvent -= OnBeginDrag;
            view.OnDragEvent -= OnDrag;
            view.OnEndDragEvent -= OnEndDrag;
            _createdViews.Remove(view);
        }
        
        private void OnViewDisposed(CubeView view)
        {
            OnViewDisposedEvent?.Invoke(view);
        }
        
        private void OnPointerDown(CubeView view, PointerEventData eventData)
        {
            OnPointerDownEvent?.Invoke(view, eventData);
        }
        
        private void OnBeginDrag(CubeView view, PointerEventData eventData)
        {
            OnBeginDragEvent?.Invoke(view, eventData);
        }
        
        private void OnDrag(CubeView view, PointerEventData eventData)
        {
            OnDragEvent?.Invoke(view, eventData);
        }
        
        private void OnEndDrag(CubeView view, PointerEventData eventData)
        {
            OnEndDragEvent?.Invoke(view, eventData);
        }
    }
}