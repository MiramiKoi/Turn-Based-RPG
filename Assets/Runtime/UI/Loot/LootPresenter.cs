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
        private readonly UIContent _uiContent;
        private readonly WorldViewDescriptions _viewDescriptions;

        public LootPresenter(LootModel model, World world, WorldViewDescriptions viewDescriptions, UIContent uiContent)
        {
            _model = model;
            _world = world;
            _viewDescriptions = viewDescriptions;
            _uiContent = uiContent;

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

            _world.TransferModel.TargetInventory.Value = unitModel.Inventory;
            _world.TransferModel.Mode.Value =
                unitModel.Description.Fraction == "trader" ? TransferMode.Trade : TransferMode.Default;

            if (unitModel.Description.Fraction == "trader")
            {
                _inventoryView.Root.AddToClassList("trade-inventory");
            }
            else
            {
                _inventoryView.Root.AddToClassList("loot-inventory");
            }

            _currentInventory = new InventoryPresenter(unitModel.Inventory, _inventoryView, _viewDescriptions,
                _uiContent, _world);
            _currentInventory.Enable();
        }

        private void Clear()
        {
            _world.TransferModel.TargetInventory.Value = null;
            _world.TransferModel.Mode.Value = TransferMode.Default;

            _inventoryView.Root.RemoveFromClassList("trade-inventory");
            _inventoryView.Root.RemoveFromClassList("loot-inventory");

            _currentInventory?.Disable();
            _currentInventory = null;
        }
    }
}