using System.Collections.Generic;

namespace Runtime.Units.Components
{
    public class ActionBlockerModel
    {
        private readonly Dictionary<UnitActionType, bool> _blocked = new();

        public void Set(UnitActionType type, bool value)
        {
            _blocked[type] = value;
        }

        public bool CanExecute(UnitActionType type)
        {
            if (_blocked.TryGetValue(UnitActionType.All, out var all) && all)
                return false;

            return !_blocked.TryGetValue(type, out var blocked) || !blocked;
        }
    }
}