using Runtime.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Landscape.Grid.Interaction
{
    public class GridInteractionPresenter : IPresenter
    {
        private readonly GridInteractionView _view;
        private readonly GridInteractionModel _model;
        private readonly World _world;

        public GridInteractionPresenter(GridInteractionModel model, GridInteractionView view, World world)
        {
            _model = model;
            _view = view;
            _world = world;
        }
        
        public void Enable()
        {
            _world.PlayerControls.Player.PointerMove.performed += OnPointerMove;
        }

        private void OnPointerMove(InputAction.CallbackContext context)
        {
            var mousePosition = context.ReadValue<Vector2>();
            
            var worldPosition = _world.MainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
            var cellPosition = _view.Tilemap.WorldToCell(worldPosition);

            var cell = _world.GridModel.GetCell(new Vector2Int(cellPosition.x, cellPosition.y));

            _model.SetCell(cell);
        }
        
        public void Disable()
        {
            _world.PlayerControls.Player.PointerMove.performed -= OnPointerMove;
        }
    }
}