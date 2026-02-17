using Runtime.Core;
using Runtime.Landscape.Grid.Cell;
using Runtime.Units;
using Runtime.Units.Actions;
using UnityEngine;

namespace Runtime.Player.Commands
{
    public class TeleportCommand : IPlayerCommand
    {
        private readonly PlayerModel _player;
        private readonly World _world;

        public TeleportCommand(PlayerModel player, World world)
        {
            _player = player;
            _world = world;
        }

        public bool CanExecute(CellModel cell)
        {
            if (cell.Unit is not UnitModel target)
                return false;

            return _player.ActionBlocker.CanExecute(UnitActionType.Move) && target.Description.Fraction == "door";
        }

        public void Execute(CellModel cell)
        {
            _world.LootModel.CancelLoot();
            _world.GridModel.ReleaseCell(_player.State.Position.Value);
            if (cell.Unit is DoorModel door)
                _player.Movement.SetPosition(door.ToPosition);
        }
    }
}