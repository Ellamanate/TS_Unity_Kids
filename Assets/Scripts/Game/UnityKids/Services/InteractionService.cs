using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class InteractionService
    {
        private readonly TowerService _towerService;
        private readonly OverlappingService _overlappingService;
        private readonly ViewsFactory _viewsFactory;
        
        public InteractionService(
            TowerService towerService,
            OverlappingService overlappingService,
            ViewsFactory viewsFactory)
        {
            _towerService = towerService;
            _overlappingService = overlappingService;
            _viewsFactory = viewsFactory;
        }
        
        public void OnEndDragShadow(CubeView shadowView, PointerEventData eventData)
        {
            var view = _viewsFactory.CreateWorldView(shadowView);
            PlaceCube(view);
        }
        
        public void OnEndDrag(CubeView view, PointerEventData eventData)
        {
            PlaceCube(view);
        }

        private void PlaceCube(CubeView view)
        {
            bool hasCubes = _towerService.HasCubes();

            if (!hasCubes && _overlappingService.IsOverlappingBasement(view.RectTransform))
            {
                _towerService.Add(view);
            }
            else if (hasCubes && _overlappingService.IsOverlappingTower(view.RectTransform))
            {
                _towerService.Add(view);
            }
            else
            {
                Object.Destroy(view.gameObject);
            }
        }
    }
}