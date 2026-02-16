using Runtime.AsyncLoad;
using Runtime.Common.ObjectPool;
using Runtime.Core;
using Runtime.UI.Player.StatusEffects;
using Runtime.Units;
using Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace Runtime.Player
{
    public class PlayerPresenter : UnitPresenter
    {
        private readonly PlayerModel _model;
        private readonly World _world;
        private readonly WorldViewDescriptions _worldViewDescriptions;

        private PlayerVisionPresenter _visionPresenter;
        private PlayerPathPresenter _pathPresenter;
        private PlayerInteractionPresenter _interactionPresenter;
        private PlayerStatusEffectsHudPresenter _statusEffectsHudPresenter;

        private LoadModel<VisualTreeAsset> _loadModelUiAsset;

        public PlayerPresenter(PlayerModel model, IObjectPool<UnitView> pool, World world,
            WorldViewDescriptions worldViewDescriptions)
            : base(model, pool, world, worldViewDescriptions)
        {
            _model = model;
            _world = world;
            _worldViewDescriptions = worldViewDescriptions;
        }

        public async override void Enable()
        {
            base.Enable();

            _world.CameraControlModel.Target.Value = View.Transform;

            _visionPresenter = new PlayerVisionPresenter(_world.GridModel, _world.UnitCollection, _model);
            _visionPresenter.Enable();

            _pathPresenter = new PlayerPathPresenter(_model, _world);
            _pathPresenter.Enable();

            _interactionPresenter = new PlayerInteractionPresenter(_model, _world);
            _interactionPresenter.Enable();

            _loadModelUiAsset = _world.AddressableModel.Load<VisualTreeAsset>(_worldViewDescriptions
                .StatusEffectViewDescriptions.StatusEffectContainerAsset.AssetGUID);
            await _loadModelUiAsset.LoadAwaiter;
            var statusEffectsView = new PlayerStatusEffectHudView(_loadModelUiAsset.Result);

            _statusEffectsHudPresenter = new PlayerStatusEffectsHudPresenter(_world.PlayerModel.Value, statusEffectsView, _world, _worldViewDescriptions);
            _statusEffectsHudPresenter.Enable();
        }

        public override void Disable()
        {
            base.Disable();

            _interactionPresenter.Disable();
            _interactionPresenter = null;

            _pathPresenter.Disable();
            _pathPresenter = null;

            _visionPresenter.Disable();
            _visionPresenter = null;

            _world.AddressableModel.Unload(_loadModelUiAsset);
            _statusEffectsHudPresenter.Disable();
            _statusEffectsHudPresenter = null;
        }
    }
}