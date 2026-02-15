using Runtime.Common;
using Runtime.Core;
using Runtime.Equipment;
using Runtime.Input;
using Runtime.UI.Inventory;
using Runtime.UI.Transfer;
using Runtime.UI.Transfer.Handlers;
using Runtime.ViewDescriptions;
using UnityEngine.InputSystem;
using TrashHandler = Runtime.UI.Transfer.Handlers.TrashHandler;

namespace Runtime.UI
{
    public class UIController : IPresenter
    {
        private readonly InventoryPresenter _playerInventoryPresenter;
        private readonly InventoryModel _playerInventoryModel;

        private readonly PlayerControls _playerControls;
        private readonly World _world;
        private readonly InventoryPresenter _trashInventoryPresenter;
        private readonly InventoryPresenter _equipmentInventory;
        private readonly EquipmentPresenter _equipmentPresenter;

        public UIController(World world, PlayerControls playerControls, WorldViewDescriptions viewDescriptions)
        {
            _world = world;
            _playerControls = playerControls;

            var router = _world.TransferRouter;
            router.Register(new TradeHandler(world.WorldDescription.ItemCollection.Descriptions["money"]));
            router.Register(new TrashHandler(world));
            router.Register(new EquipmentHandler(world.PlayerModel));
            router.Register(new TransferHandler());
            router.Register(new SwapHandler());

            _playerInventoryModel = world.PlayerModel.Inventory;
            var inventoryView = new InventoryView(viewDescriptions.InventoryViewDescription.InventoryAsset);
            inventoryView.Root.AddToClassList(UIConstants.Inventory.PlayerInventoryStyle);
            _playerInventoryPresenter = new InventoryPresenter(_playerInventoryModel, inventoryView, viewDescriptions,
                world, InventoryType.Player);

            var equipmentModel = world.PlayerModel.Equipment.Inventory;
            var equipmentView = new EquipmentView(viewDescriptions.InventoryViewDescription.EquipmentAsset);
            equipmentView.Root.AddToClassList(UIConstants.Inventory.EquipmentInventoryStyle);
            _equipmentInventory = new InventoryPresenter(equipmentModel, equipmentView, viewDescriptions, world,
                InventoryType.Equipment);
            _equipmentPresenter = new EquipmentPresenter(world.PlayerModel, equipmentView);

            var trashInventoryModel = new InventoryModel(UIConstants.Inventory.TrashInventorySize);
            var trashInventoryView = new InventoryView(viewDescriptions.InventoryViewDescription.InventoryAsset);
            trashInventoryView.Root.AddToClassList(UIConstants.Inventory.TrashInventoryStyle);
            _trashInventoryPresenter = new InventoryPresenter(trashInventoryModel, trashInventoryView, viewDescriptions,
                world, InventoryType.Trash);
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

            HideInventory();
        }

        private void ToggleInventory(InputAction.CallbackContext context)
        {
            if (_playerInventoryModel.Enabled)
            {
                HideInventory();
            }
            else
            {
                ShowInventory();
            }
        }

        private void OpenInventory(IUnit unit)
        {
            if (_playerInventoryModel.Enabled)
            {
                return;
            }

            ShowInventory();
        }

        private void ShowInventory()
        {
            _playerInventoryPresenter.Enable();
            _equipmentInventory.Enable();
            _equipmentPresenter.Enable();
            _trashInventoryPresenter.Enable();
        }

        private void HideInventory()
        {
            _playerInventoryPresenter.Disable();
            _equipmentInventory.Disable();
            _equipmentPresenter.Disable();
            _trashInventoryPresenter.Disable();
        }
    }
}