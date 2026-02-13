using Runtime.Common;
using Runtime.Core;
using Runtime.Equipment;
using Runtime.Input;
using Runtime.UI.Inventory;
using Runtime.UI.Trade;
using Runtime.UI.Transfer;
using Runtime.UI.TrashInventory;
using Runtime.ViewDescriptions;
using UniRx;
using UnityEngine.InputSystem;

namespace Runtime.UI
{
    public class UIController : IPresenter
    {
        private TransferPresenter _transferPresenter;
        private readonly InventoryPresenter _playerInventory;
        private readonly InventoryModel _inventoryModel;
        private readonly PlayerControls _playerControls;
        private readonly World _world;
        private readonly CompositeDisposable _disposables = new();
        private readonly TrashInventoryPresenter _trashInventoryPresenter;

        private readonly InventoryModel _equipmentModel;
        private readonly InventoryPresenter _equipmentInventory;
        private readonly EquipmentPresenter _equipmentPresenter;
        
        public UIController(World world, PlayerControls playerControls, WorldViewDescriptions viewDescriptions)
        {
            _world = world;
            _playerControls = playerControls;
            
            _inventoryModel = world.PlayerModel.Inventory;
            var inventoryView = new InventoryView(viewDescriptions.InventoryViewDescription.InventoryAsset);
            inventoryView.Root.AddToClassList("player-inventory");
            
            _playerInventory = new InventoryPresenter(_inventoryModel, inventoryView, viewDescriptions, world);
            
            _equipmentModel = world.PlayerModel.Equipment.Inventory;
            
            var equipmentView = new InventoryView(viewDescriptions.InventoryViewDescription.InventoryAsset);
            equipmentView.Root.AddToClassList("equipment-inventory");
            _equipmentInventory = new InventoryPresenter(_equipmentModel, equipmentView, viewDescriptions, world);
            _equipmentPresenter = new EquipmentPresenter(_world.PlayerModel, _world);
            
            var trashInventoryView = new InventoryView(viewDescriptions.InventoryViewDescription.InventoryAsset);
            trashInventoryView.Root.AddToClassList("trash-inventory");
            var trashInventoryModel = new InventoryModel(1);
            
            _trashInventoryPresenter = new TrashInventoryPresenter(trashInventoryView, trashInventoryModel, viewDescriptions, viewDescriptions.UIContent, world);
        }

        public void Enable()
        {
            _playerControls.Gameplay.ToggleInventory.performed += ToggleInventory;
            _world.LootModel.OnLootRequested += OpenInventory;

            _world.TransferModel.Mode.Subscribe(OnModeChanged).AddTo(_disposables);
        }

        public void Disable()
        {
            _playerControls.Gameplay.ToggleInventory.performed -= ToggleInventory;
            _world.LootModel.OnLootRequested -= OpenInventory;

            _transferPresenter?.Disable();
            _transferPresenter = null;

            _disposables.Clear();
        }
        
        private void OnModeChanged(TransferMode mode)
        {
            _transferPresenter?.Disable();

            _transferPresenter = mode switch
            {
                TransferMode.Trade => new TradePresenter(_world.TransferModel, _world),
                TransferMode.Trash => new TrashPresenter(_world.TransferModel, _world),
                _ => new TransferPresenter(_world.TransferModel)
            };

            _transferPresenter.Enable();
        }

        private void ToggleInventory(InputAction.CallbackContext context)
        {
            if (_inventoryModel.Enabled)
            {
                HideInventory();
                _trashInventoryPresenter.Disable();
            }
            else
            {
                ShowInventory();
                _trashInventoryPresenter.Enable();
            }
        }

        private void OpenInventory(IUnit unit)
        {
            if (_inventoryModel.Enabled)
            {
                return;
            }

            ShowInventory();
        }

        private void ShowInventory()
        {
            _world.TransferModel.SourceInventory.Value = _inventoryModel;
            _trashInventoryPresenter.Enable();
            _playerInventory.Enable();
            _equipmentInventory.Enable();
            _equipmentPresenter.Enable();
        }

        private void HideInventory()
        {
            _world.TransferModel.SourceInventory.Value = null;
            _trashInventoryPresenter.Disable();
            _playerInventory.Disable();
            _equipmentInventory.Disable();
            _equipmentPresenter.Disable();
        }
    }
}