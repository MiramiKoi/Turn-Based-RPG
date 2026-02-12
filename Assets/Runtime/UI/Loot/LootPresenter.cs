using Runtime.Common;
using Runtime.Core;
using Runtime.UI.Inventory;
using Runtime.UI.Trade;
using Runtime.UI.Transfer;
using Runtime.Units;
using Runtime.ViewDescriptions;

namespace Runtime.UI.Loot
{
    public class LootPresenter : IPresenter
    {
        private InventoryPresenter _currentInventory;

        private readonly World _world;
        private readonly UIContent _uiContent;
        private readonly InventoryView _inventoryView;
        private readonly WorldViewDescriptions _viewDescriptions;
        private TransferPresenter _transferPresenter;

        public LootPresenter(World world, UIContent uiContent, WorldViewDescriptions viewDescriptions)
        {
            _world = world;
            _uiContent = uiContent;
            _viewDescriptions = viewDescriptions;

            _inventoryView = new InventoryView(_viewDescriptions.InventoryViewDescription.InventoryAsset);
        }

        public void Enable()
        {
            _world.LootModel.OnLootRequested += Show;
            _world.TurnBaseModel.OnPlayerStepFinished += Clear;
        }

        public void Disable()
        {
            _world.LootModel.OnLootRequested -= Show;
            _world.TurnBaseModel.OnPlayerStepFinished -= Clear;
        }

        private void Show(IUnit unit)
        {
            if (unit is not UnitModel unitModel)
            {
                return;
            }
            
            Clear();
                
            if (unitModel.Description.Fraction == "trader")
            {
                _inventoryView.Root.AddToClassList("trade-inventory");
                _transferPresenter = new TradePresenter(_world.TransferModel, _world);
            }
            else
            {
                _inventoryView.Root.AddToClassList("loot-inventory");
                _transferPresenter = new TransferPresenter(_world.TransferModel);
            }

            _transferPresenter.Enable();
                
            _currentInventory = new InventoryPresenter(unitModel.InventoryModel, _inventoryView, _viewDescriptions,
                _uiContent, _world);
                
            _currentInventory.Enable();
        }

        private void Clear()
        {
            _inventoryView.Root.RemoveFromClassList("trade-inventory");
            _inventoryView.Root.RemoveFromClassList("loot-inventory");
            
            _transferPresenter?.Disable();
            _transferPresenter = null;
            
            _currentInventory?.Disable();
            _currentInventory = null;
        }
    }
}