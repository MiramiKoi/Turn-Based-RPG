using System.Collections.Generic;
using Runtime.Units;

namespace Runtime.TurnBase
{
    public class BattleModel
    {
        private readonly Dictionary<string, UnitModel> _units = new();

        public void EnterToBattle(UnitModel unit)
        {
            _units.Add(unit.Id, unit);
        }

        public void LeaveToBattle(UnitModel unit)
        {
            _units.Remove(unit.Id);
        }

        public bool IsInBattle() => _units.Count > 0;
    }
}