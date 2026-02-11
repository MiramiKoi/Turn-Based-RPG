using Runtime.Common;
using Runtime.Core;
using Runtime.Input;
using Runtime.UI.Inventory;
using Runtime.ViewDescriptions;
using UnityEngine.InputSystem;

namespace Runtime.UI
{
    public class UIController : IPresenter
    {
        private readonly InventoryPresenter _playerInventory;
        private readonly InventoryModel _inventoryModel;
        private readonly PlayerControls _playerControls;
        private readonly World _world;

        public UIController(World world, PlayerControls playerControls, WorldViewDescriptions viewDescriptions,
            UIContent uiContent)
        {
            _world = world;
            _playerControls = playerControls;

            _inventoryModel = world.UnitCollection.Get("character").InventoryModel;

            var inventoryView = new InventoryView(viewDescriptions.InventoryViewDescription.InventoryAsset);
            inventoryView.Root.AddToClassList("player-inventory");

            _playerInventory =
                new InventoryPresenter(_inventoryModel, inventoryView, viewDescriptions, uiContent, _world);
        }

        public void Enable()
        {
            _playerControls.Gameplay.ToggleInventory.performed += ToggleInventory;
            _world.LootModel.OnLootRequested += OpenInventory;
        }

        public void Disable()
        {
            _playerControls.Gameplay.ToggleInventory.performed -= ToggleInventory;
            _world.LootModel.OnLootRequested -= OpenInventory;
        }

        private void ToggleInventory(InputAction.CallbackContext context)
        {
            if (_inventoryModel.Enabled)
            {
                _playerInventory.Disable();
            }
            else
            {
                _playerInventory.Enable();
            }
        }

        private void OpenInventory(IUnit unit)
        {
            if (_inventoryModel.Enabled)
            {
                return;
            }

            _playerInventory.Enable();
        }
    }
}