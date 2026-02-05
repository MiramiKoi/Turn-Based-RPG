using Runtime.Input;
using Runtime.UI.Inventory;
using Runtime.ViewDescriptions;
using UnityEngine.InputSystem;

namespace Runtime.UI
{
    public class UIController
    {
        private readonly InventoryPresenter _inventoryPresenter;
        private readonly InventoryModel _inventoryModel;
        private const int InventorySize = 16;

        private readonly PlayerControls _playerControls;

        public UIController(PlayerControls playerControls, WorldViewDescriptions viewDescriptions, UIContent uiContent)
        {
            _playerControls = playerControls;

            var inventoryView = new InventoryView(viewDescriptions.InventoryViewDescription.InventoryAsset);
            _inventoryModel = new InventoryModel(InventorySize);
            _inventoryPresenter = new InventoryPresenter(_inventoryModel,  inventoryView, viewDescriptions, uiContent);
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
            if (_inventoryModel.Enabled)
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