using System.Collections.Generic;
using Runtime.Units;

namespace Runtime.TurnBase.Battle
{
    public class BattleModel
    {
        private readonly Dictionary<string, UnitModel> _units = new();

        public void EnterToBattle(UnitModel unit)
        {
            _units.TryAdd(unit.Id, unit);
        }

        public void LeaveBattle(UnitModel unit)
        {
            _units.Remove(unit.Id);
        }

        public bool IsInBattle()
        {
            return _units.Count > 0;
        }
    }
}