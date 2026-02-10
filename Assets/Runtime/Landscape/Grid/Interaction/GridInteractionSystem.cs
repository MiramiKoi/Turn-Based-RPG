using Runtime.Core;
using Runtime.GameSystems;
using Runtime.Landscape.Grid.Indication;
using UnityEngine;

namespace Runtime.Landscape.Grid.Interaction
{
    public class GridInteractionSystem : IGameSystem
    {
        public string Id => "grid-interaction-system";

        private readonly GridInteractionModel _model;
        private readonly GridView _view;
        private readonly World _world;

        public GridInteractionSystem(GridInteractionModel model, GridView view, World world)
        {
            _model = model;
            _view = view;
            _world = world;
        }

        public void Update(float deltaTime)
        {
            if (_model.IsActive.Value)
            {
                var mousePosition = _world.PlayerControls.Gameplay.PointerPosition.ReadValue<Vector2>();

                var worldPosition =
                    _world.MainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
                var nextCellPosition = _view.IndicationTilemap.WorldToCell(worldPosition);
                if (nextCellPosition is { x: < GridConstants.Width, y: < GridConstants.Height } and
                    { x: >= 0, y: >= 0 })
                {
                    var cell = _world.GridModel.GetCell(new Vector2Int(nextCellPosition.x, nextCellPosition.y));

                    if (cell != _model.CurrentCell)
                    {
                        if (_model.CurrentCell != null)
                        {
                            _world.GridModel.Cells[_model.CurrentCell.Position.x, _model.CurrentCell.Position.y]
                                .SetIndication(IndicationType.Null);
                        }

                        _model.SetCell(cell);
                        _world.GridModel.Cells[nextCellPosition.x, nextCellPosition.y]
                            .SetIndication(IndicationType.Cursor);
                    }
                }
            }
        }
    }
}