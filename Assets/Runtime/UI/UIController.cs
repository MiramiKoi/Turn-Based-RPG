using Runtime.Core;
using Runtime.Input;
using Runtime.UI.Inventory;
using Runtime.ViewDescriptions;
using UnityEngine.InputSystem;

namespace Runtime.UI
{
    public class UIController
    {
        private readonly InventoryPresenter _inventoryPresenter;
        private readonly World _world;

        private readonly PlayerControls _playerControls;

        public UIController(World world, PlayerControls playerControls, WorldViewDescriptions viewDescriptions,
            UIContent uiContent)
        {
            _world = world;
            _playerControls = playerControls;

            var inventoryView = new InventoryView(viewDescriptions.InventoryViewDescription.InventoryAsset);
            _inventoryPresenter = new InventoryPresenter(_world.InventoryModel, inventoryView, viewDescriptions,
                uiContent, _world);
        }

        public void Enable()
        {
            _playerControls.Gameplay.ToggleInventory.performed += ToggleInventory;
        }

        public void Disable()
        {
            _playerControls.Gameplay.ToggleInventory.performed -= ToggleInventory;
        }

        private void ToggleInventory(InputAction.CallbackContext context)
        {
            if (_world.InventoryModel.Enabled)
            {
                _inventoryPresenter.Disable();
            }
            else
            {
                _inventoryPresenter.Enable();
            }
        }
    }
}