using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class CubeView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<CubeView, PointerEventData> OnBeginDragEvent;
        public event Action<CubeView, PointerEventData> OnDragEvent;
        public event Action<CubeView, PointerEventData> OnEndDragEvent;

        [field: SerializeField] public RectTransform RectTransform { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
        
        public void SetColor(Color color)
        {
            Color = color;
            Image.color = color;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragEvent?.Invoke(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent?.Invoke(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent?.Invoke(this, eventData);
        }
    }
}