using Runtime.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Landscape.Grid.Interaction
{
    public class GridInteractionPresenter : IPresenter
    {
        private readonly GridView _view;
        private readonly GridInteractionModel _model;
        private readonly World _world;

        public GridInteractionPresenter(GridInteractionModel model, GridView view, World world)
        {
            _model = model;
            _view = view;
            _world = world;
        }

        public void Enable()
        {
            _world.PlayerControls.Player.PointerMove.performed += OnPointerMove;
        }
        
        public void Disable()
        {
            _world.PlayerControls.Player.PointerMove.performed -= OnPointerMove;
            Clear();
        }

        private void OnPointerMove(InputAction.CallbackContext context)
        {
            var mousePosition = context.ReadValue<Vector2>();

            var worldPosition = _world.MainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
            var cellPosition = _view.Tilemap.WorldToCell(worldPosition);

            if (cellPosition is { x: < GridConstants.Width, y: < GridConstants.Height } and { x: >= 0, y: >= 0 })
            {
                var cell = _world.GridModel.GetCell(new Vector2Int(cellPosition.x, cellPosition.y));
                _model.SetCell(cell);
            }
            else
            {
                Clear();
            }
        }

        private void Clear()
        {
            _model.SetCell(null);
        }
    }
}