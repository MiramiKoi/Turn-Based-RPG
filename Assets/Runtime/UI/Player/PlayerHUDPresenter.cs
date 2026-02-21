using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.Equipment;
using Runtime.Player;
using Runtime.UI.Inventory;
using Runtime.UI.Player.StatusEffects;
using Runtime.UI.Transfer;
using Runtime.UI.Transfer.Handlers;
using Runtime.ViewDescriptions;
using UniRx;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Runtime.UI.Player
{
    public class PlayerHUDPresenter : IPresenter
    {
        private readonly World _world;
        private readonly WorldViewDescriptions _viewDescriptions;
        private readonly CompositeDisposable _disposables = new();

        private InventoryView _inventoryView;
        private InventoryPresenter _inventoryPresenter;
        
        private EquipmentView _equipmentView;
        private InventoryPresenter _equipmentInventoryPresenter;
        private EquipmentPresenter _equipmentPresenter;

        private InventoryView _trashInventoryView;
        private ITransferHandler _transferHandler;
        private InventoryPresenter _trashInventoryPresenter;

        private LoadModel<VisualTreeAsset> _loadModelUiAsset;
        private PlayerStatusEffectHudView _statusEffectHudView;
        private PlayerStatusEffectsHudPresenter _statusEffectsHudPresenter;


        public PlayerHUDPresenter(World world, WorldViewDescriptions viewDescriptions)
        {
            _world = world;
            _viewDescriptions = viewDescriptions;
        }

        public async void Enable()
        {
            _world.PlayerControls.Gameplay.ToggleInventory.performed += HandleToggleInventory;
            _world.LootModel.OnLootRequested += HandleOpenInventory;
            _world.PlayerModel.SkipLatestValueOnSubscribe().Subscribe(HandlePlayerChanged).AddTo(_disposables);
            
            _inventoryView = new InventoryView(_viewDescriptions.InventoryViewDescription.InventoryAsset);
            _inventoryView.Root.AddToClassList(UIConstants.Inventory.PlayerInventoryStyle);
            
            _equipmentView = new EquipmentView(_viewDescriptions.InventoryViewDescription.EquipmentAsset);
            _equipmentView.Root.AddToClassList(UIConstants.Inventory.EquipmentInventoryStyle);
            
            _trashInventoryView = new InventoryView(_viewDescriptions.InventoryViewDescription.InventoryAsset);
            _trashInventoryView.Root.AddToClassList(UIConstants.Inventory.TrashInventoryStyle);
            
            _loadModelUiAsset = _world.AddressableModel.Load<VisualTreeAsset>(_viewDescriptions
                .StatusEffectViewDescriptions.StatusEffectContainerAsset.AssetGUID);
            await _loadModelUiAsset.LoadAwaiter;
            _statusEffectHudView = new PlayerStatusEffectHudView(_loadModelUiAsset.Result);
        }

        public void Disable()
        {
            _world.PlayerControls.Gameplay.ToggleInventory.performed -= HandleToggleInventory;
            _world.LootModel.OnLootRequested -= HandleOpenInventory;
            
            _disposables.Dispose();

            HideInventory();
        }
        
        private void ShowInventory()
        {
            _inventoryPresenter.Enable();
            _equipmentInventoryPresenter.Enable();
            _equipmentPresenter.Enable();
            _trashInventoryPresenter.Enable();
        }

        private void HideInventory()
        {
            _inventoryPresenter.Disable();
            _equipmentInventoryPresenter.Disable();
            _equipmentPresenter.Disable();
            _trashInventoryPresenter.Disable();
        }

        private void HandleToggleInventory(InputAction.CallbackContext context)
        {
            if (_world.PlayerModel.Value.Inventory.Enabled)
            {
                HideInventory();
            }
            else
            {
                ShowInventory();
            }
        }

        private void HandleOpenInventory(IUnit unit)
        {
            if (_world.PlayerModel.Value.Inventory.Enabled)
            {
                return;
            }

            ShowInventory();
        }

        private void ClearPresenter()
        {
            _inventoryPresenter?.Disable();
            _inventoryPresenter = null;
            
            _equipmentInventoryPresenter?.Disable();
            _equipmentInventoryPresenter = null;
            
            _equipmentPresenter?.Disable();
            _equipmentPresenter = null;
            
            _trashInventoryPresenter?.Disable();
            _trashInventoryPresenter = null;
            
            _statusEffectsHudPresenter?.Disable();
            _statusEffectsHudPresenter = null;
        }

        private void HandlePlayerChanged(PlayerModel player)
        {
            ClearPresenter();

            if (_transferHandler != null)
            {
                _world.TransferRouter.Unregister(_transferHandler);
            }

            _transferHandler = new EquipmentHandler(player);
            _world.TransferRouter.Register(_transferHandler);

            
            _inventoryPresenter = new InventoryPresenter(player.Inventory, _inventoryView, _viewDescriptions,
                _world, InventoryType.Player);
            
            var equipmentModel = _world.PlayerModel.Value.Equipment.Inventory;
            _equipmentInventoryPresenter = new InventoryPresenter(equipmentModel, _equipmentView, _viewDescriptions, _world,
                InventoryType.Equipment);
            _equipmentPresenter = new EquipmentPresenter(_world.PlayerModel.Value, _equipmentView);
            
            var trashInventoryModel = new InventoryModel(UIConstants.Inventory.TrashInventorySize);
            _trashInventoryPresenter = new InventoryPresenter(trashInventoryModel, _trashInventoryView, _viewDescriptions,
                _world, InventoryType.Trash);
            
            _statusEffectsHudPresenter = new PlayerStatusEffectsHudPresenter(_world.PlayerModel.Value, _statusEffectHudView, _world, _viewDescriptions);
            _statusEffectsHudPresenter.Enable();
        }
    }
}