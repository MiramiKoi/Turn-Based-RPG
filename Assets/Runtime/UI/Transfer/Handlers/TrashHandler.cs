using Runtime.Core;
using Runtime.Units;
using UnityEngine;

namespace Runtime.UI.Transfer.Handlers
{
    public class TrashHandler : BaseTransferHandler
    {
        private readonly World _world;

        public TrashHandler(World world)
        {
            _world = world;
        }

        public override bool CanHandle(TransferModel context)
        {
            return context.TargetType == InventoryType.Trash;
        }

        public override void Handle(TransferModel context)
        {
            if (context.SourceCell?.ItemDescription == null)
            {
                return;
            }

            var item = context.SourceCell.ItemDescription;
            var amount = context.SourceCell.Amount;

            context.SourceCell.TryTake(amount);

            var position = _world.PlayerModel.Value.State.Position.Value;
            var toPosition = position + Vector2Int.down;

            var cellUnit = _world.GridModel.GetCell(toPosition).Unit;

            if (cellUnit is UnitModel { Description: { Id: "loot" } } lootUnit)
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