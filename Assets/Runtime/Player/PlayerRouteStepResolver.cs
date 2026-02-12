using Runtime.Core;
using Runtime.Player.Commands;
using Runtime.Units;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerRouteStepResolver
    {
        private readonly World _world;

        public PlayerRouteStepResolver(World world)
        {
            _world = world;
        }

        public IPlayerCommand Resolve(PlayerModel player, Vector2Int targetCell)
        {
            var cell = _world.GridModel.GetCell(targetCell);

            if (cell.Unit == null)
            {
                return new MoveCommand(player, _world, targetCell);
            }

            var unit = (UnitModel)cell.Unit;

            if (unit == player)
                return null;

            if (unit.IsDead || unit.Description.Fraction == "trader")
                return new LootCommand(_world, unit);

            if (unit.Description.Fraction != player.Description.Fraction)
                return new AttackCommand(player, unit, _world);

            return null;
        }
    }
}