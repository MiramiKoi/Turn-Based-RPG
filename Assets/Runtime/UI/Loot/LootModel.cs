using System;
using Runtime.Common;

namespace Runtime.UI.Loot
{
    public class LootModel
    {
        public event Action<IUnit> OnLootRequested;
        public event Action OnLootCanceled;
        
        public void RequestLoot(IUnit unit)
        {
            OnLootRequested?.Invoke(unit);
        }

        public void CancelLoot()
        {
            OnLootCanceled?.Invoke();
        }
    }
}