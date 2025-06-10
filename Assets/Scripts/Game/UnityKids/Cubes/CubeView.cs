using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class CubeView : MonoBehaviour,
        IDisposable,
        IPointerDownHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        public event Action<CubeView> OnDestroyEvent;
        public event Action<CubeView> OnDisposedEvent;
        public event Action<CubeView, PointerEventData> OnPointerDownEvent;
        public event Action<CubeView, PointerEventData> OnBeginDragEvent;
        public event Action<CubeView, PointerEventData> OnDragEvent;
        public event Action<CubeView, PointerEventData> OnEndDragEvent;
        
        [SerializeField] private Image _image;
        [SerializeField] private Image _innerImage;
        
        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }

        private bool _isDisposed;

        public void Dispose()
        {
            _isDisposed = true;
            OnDisposedEvent?.Invoke(this);
        }

        private void OnDestroy()
        {
            OnDestroyEvent?.Invoke(this);
        }
        
        public void SetColor(Color color)
        {
            Color = color;
            _image.color = color;
        }
        
        public void SetMaterial(Material material)
        {
            _image.material = material;
            _innerImage.material = material;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isDisposed)
            {
                OnPointerDownEvent?.Invoke(this, eventData);
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_isDisposed)
            {
                OnBeginDragEvent?.Invoke(this, eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDisposed)
            {
                OnDragEvent?.Invoke(this, eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDisposed)
            {
                OnEndDragEvent?.Invoke(this, eventData);
            }
        }
    }
}