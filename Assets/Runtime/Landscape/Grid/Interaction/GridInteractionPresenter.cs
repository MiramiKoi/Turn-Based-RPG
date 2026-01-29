using Runtime.Common;
using Runtime.Landscape.Grid.Indication;
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
            _world.PlayerControls.Gameplay.PointerMove.performed += OnPointerMove;
        }

        public void Disable()
        {
            _world.PlayerControls.Gameplay.PointerMove.performed -= OnPointerMove;
            Clear();
        }

        private void OnPointerMove(InputAction.CallbackContext context)
        {
            var mousePosition = context.ReadValue<Vector2>();

            var worldPosition = _world.MainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
            var nextCellPosition = _view.Tilemap.WorldToCell(worldPosition);
            Clear();
            if (nextCellPosition is { x: < GridConstants.Width, y: < GridConstants.Height } and { x: >= 0, y: >= 0 })
            {
                var cell = _world.GridModel.GetCell(new Vector2Int(nextCellPosition.x, nextCellPosition.y));
                _model.SetCell(cell);
                _world.GridModel.Cells[nextCellPosition.x, nextCellPosition.y].SetIndication(IndicationType.Cursor);
            }
        }

        private void Clear()
        {
            if (_model.CurrentCell != null)
                _world.GridModel.Cells[_model.CurrentCell.Position.x, _model.CurrentCell.Position.y].SetIndication(IndicationType.Null);
            _model.SetCell(null);
        }
    }
}