using System;
using Runtime.Common;
using Runtime.Landscape.Grid.Cell;

namespace Runtime.UI.Loot
{
    public class LootModel
    {
        public event Action<IUnit> OnLootRequested;

        public void RequestLoot(IUnit unit)
        {
            OnLootRequested?.Invoke(unit);
        }
    }
}