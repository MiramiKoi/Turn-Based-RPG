using Runtime.Common;
using Runtime.Core;
using Runtime.UI.Inventory;
using Runtime.UI.Transfer;
using Runtime.Units;
using Runtime.ViewDescriptions;

namespace Runtime.UI.Loot
{
    public class LootPresenter : IPresenter
    {
        private InventoryPresenter _currentInventory;

        private readonly LootModel _model;
        private readonly InventoryView _inventoryView;
        private readonly World _world;
        private readonly WorldViewDescriptions _viewDescriptions;

        public LootPresenter(LootModel model, World world, WorldViewDescriptions viewDescriptions)
        {
            _model = model;
            _world = world;
            _viewDescriptions = viewDescriptions;

            _inventoryView = new InventoryView(_viewDescriptions.InventoryViewDescription.InventoryAsset);
        }

        public void Enable()
        {
            _model.OnLootRequested += Show;
            _model.OnLootCanceled += Clear;
        }

        public void Disable()
        {
            _model.OnLootRequested -= Show;
            _model.OnLootCanceled -= Clear;
            Clear();
        }

        private void Show(IUnit unit)
        {
            if (unit is not UnitModel unitModel)
            {
                return;
            }

            Clear();

            var isTrader = unitModel.Description.Fraction == UnitsConstants.TraderFraction;
            var inventoryType = isTrader ? InventoryType.Trader : InventoryType.Loot;

            _inventoryView.Root.AddToClassList(isTrader ? UIConstants.Inventory.TradeInventoryStyle : UIConstants.Inventory.LootInventoryStyle);

            _currentInventory = new InventoryPresenter(unitModel.Inventory, _inventoryView, _viewDescriptions, _world, inventoryType);
            _currentInventory.Enable();
        }

        private void Clear()
        {
            _inventoryView.Root.RemoveFromClassList(UIConstants.Inventory.TradeInventoryStyle);
            _inventoryView.Root.RemoveFromClassList(UIConstants.Inventory.LootInventoryStyle);

            _currentInventory?.Disable();
            _currentInventory = null;
        }
    }
}