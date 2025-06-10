using UnityEngine;

namespace Game
{
    public class InteractionProxy
    {
        private readonly InteractionService _interactionService;
        private readonly TowerService _towerService;
        private readonly ViewsFactory _viewsFactory;
        private readonly SpawningService _spawningService;
        private readonly RectTransform _canvasTransform;

        private CubeView _viewShadow;
        
        public InteractionProxy(
            InteractionService interactionService,
            TowerService towerService,
            ViewsFactory viewsFactory,
            SpawningService spawningService,
            RectTransform canvasTransform)
        {
            _interactionService = interactionService;
            _towerService = towerService;
            _viewsFactory = viewsFactory;
            _spawningService = spawningService;
            _canvasTransform = canvasTransform;
        }
        
        public void OnBeginDrag(CubeView view)
        {
            if (!_towerService.IsInTower(view))
            {
                _viewShadow = _viewsFactory.CreateFrom(view);
                _viewShadow.RectTransform.SetParent(_canvasTransform);
                _viewShadow.RectTransform.SetSiblingIndex(-1);
            }
            else
            {
                _interactionService.OnBeginDrag(view);
            }
        }

        public void OnDrag(CubeView view, Vector3 newPosition)
        {
            var dragTarget = _viewShadow != null ? _viewShadow : view;
            _interactionService.OnDrag(dragTarget, newPosition);
        }
        
        public void OnEndDrag(CubeView view)
        {
            if (_viewShadow != null)
            {
                var newView = _spawningService.CreateWorldView(_viewShadow);
                _interactionService.OnEndDrag(newView);
                Object.Destroy(_viewShadow.gameObject);
                _viewShadow = null;
            }
            else
            {
                _interactionService.OnEndDrag(view);
            }
        }
    }
}