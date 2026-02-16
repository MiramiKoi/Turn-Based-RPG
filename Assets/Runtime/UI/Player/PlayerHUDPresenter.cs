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
        
        private InventoryPresenter _playerInventoryPresenter;
        private InventoryPresenter _trashInventoryPresenter;
        private InventoryPresenter _equipmentInventoryPresenter;
        private EquipmentPresenter _equipmentPresenter;

        private ITransferHandler _transferHandler;
        
        private LoadModel<VisualTreeAsset> _loadModelUiAsset;
        private PlayerStatusEffectHudView _playerStatusEffectHudPresenter;
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
            
            _loadModelUiAsset = _world.AddressableModel.Load<VisualTreeAsset>(_viewDescriptions
                .StatusEffectViewDescriptions.StatusEffectContainerAsset.AssetGUID);
            await _loadModelUiAsset.LoadAwaiter;
            _playerStatusEffectHudPresenter = new PlayerStatusEffectHudView(_loadModelUiAsset.Result);
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
            _playerInventoryPresenter.Enable();
            _equipmentInventoryPresenter.Enable();
            _equipmentPresenter.Enable();
            _trashInventoryPresenter.Enable();
        }

        private void HideInventory()
        {
            _playerInventoryPresenter.Disable();
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
            _playerInventoryPresenter?.Disable();
            _playerInventoryPresenter = null;
            
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

            var inventoryView = new InventoryView(_viewDescriptions.InventoryViewDescription.InventoryAsset);
            inventoryView.Root.AddToClassList(UIConstants.Inventory.PlayerInventoryStyle);
            _playerInventoryPresenter = new InventoryPresenter(player.Inventory, inventoryView, _viewDescriptions,
                _world, InventoryType.Player);
            
            var equipmentModel = _world.PlayerModel.Value.Equipment.Inventory;
            var equipmentView = new EquipmentView(_viewDescriptions.InventoryViewDescription.EquipmentAsset);
            equipmentView.Root.AddToClassList(UIConstants.Inventory.EquipmentInventoryStyle);
            _equipmentInventoryPresenter = new InventoryPresenter(equipmentModel, equipmentView, _viewDescriptions, _world,
                InventoryType.Equipment);
            _equipmentPresenter = new EquipmentPresenter(_world.PlayerModel.Value, equipmentView);
            
            var trashInventoryModel = new InventoryModel(UIConstants.Inventory.TrashInventorySize);
            var trashInventoryView = new InventoryView(_viewDescriptions.InventoryViewDescription.InventoryAsset);
            trashInventoryView.Root.AddToClassList(UIConstants.Inventory.TrashInventoryStyle);
            _trashInventoryPresenter = new InventoryPresenter(trashInventoryModel, trashInventoryView, _viewDescriptions,
                _world, InventoryType.Trash);
            
            _statusEffectsHudPresenter = new PlayerStatusEffectsHudPresenter(_world.PlayerModel.Value, _playerStatusEffectHudPresenter, _world, _viewDescriptions);
            _statusEffectsHudPresenter.Enable();
        }
    }
}