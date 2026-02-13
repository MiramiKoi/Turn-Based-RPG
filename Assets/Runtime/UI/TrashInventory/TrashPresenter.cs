using Runtime.Core;
using Runtime.UI.Transfer;
using Runtime.Units;
using UnityEngine;

namespace Runtime.UI.TrashInventory
{
    public class TrashPresenter : TransferPresenter
    {
        private readonly World _world;

        public TrashPresenter(TransferModel model, World world) : base(model)
        {
            _world = world;
        }

        protected override void Execute(bool targetIsSource)
        {
            var sourceCellIsFromSource = _currentSource?.Cells.Contains(_model.SourceCell) == true;
            var targetCellIsFromSource = _currentSource?.Cells.Contains(_model.TargetCell) == true;

            if (sourceCellIsFromSource == targetCellIsFromSource)
            {
                base.Execute(targetIsSource);
                return;
            }

            if (_model.SourceCell?.ItemDescription == null)
            {
                return;
            }

            var item = _model.SourceCell.ItemDescription;
            var amount = _model.SourceCell.Amount;

            _model.SourceCell.TryTake(amount);

            var position = _world.PlayerModel.State.Position.Value;
            var toPosition = position + Vector2Int.down;
            
            var lootCell = _world.GridModel.GetCell(toPosition).Unit;
            
            if (lootCell is UnitModel { Description: { Id: "loot" } } lootUnit)
            {
                lootUnit.Inventory.TryPutItem(item, amount);
                return;
            }
            
            var unit = _world.UnitCollection.Create("loot");
            unit.Inventory.TryPutItem(item, amount);
            
            unit.Movement.SetPosition(toPosition);
        }
    }
}