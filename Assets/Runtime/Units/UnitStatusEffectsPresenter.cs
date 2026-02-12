using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.StatusEffects.Applier;
using Runtime.StatusEffects.Collection;
using Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace Runtime.Units
{
    public class UnitStatusEffectsPresenter : IPresenter
    {
        private readonly UnitModel _model;
        private readonly UnitView _view;
        private readonly World _world;
        private readonly WorldViewDescriptions _viewDescriptions;

        private StatusEffectCollectionPresenter _statusEffectsPresenter;
        private StatusEffectApplierPresenter _statusEffectApplierPresenter;
        private LoadModel<VisualTreeAsset> _statusEffectsLoadModel;

        public UnitStatusEffectsPresenter(UnitModel model, UnitView view, World world, WorldViewDescriptions viewDescriptions)
        {
            _model = model;
            _view = view;
            _world = world;
            _viewDescriptions = viewDescriptions;
        }

        public async void Enable()
        {
            _statusEffectApplierPresenter = new StatusEffectApplierPresenter(_model.Effects, _model, _world);
            _statusEffectApplierPresenter.Enable();

            _statusEffectsLoadModel = _world.AddressableModel.Load<VisualTreeAsset>(
                _viewDescriptions.StatusEffectViewDescriptions.StatusEffectContainerAsset.AssetGUID);
            await _statusEffectsLoadModel.LoadAwaiter;

            _statusEffectsPresenter = new StatusEffectCollectionPresenter(_model, _view, _world, _viewDescriptions);
            _statusEffectsPresenter.Enable();
        }

        public void Disable()
        {
            _world.AddressableModel.Unload(_statusEffectsLoadModel);
            _statusEffectsPresenter.Disable();
            _statusEffectApplierPresenter.Disable();
        }
    }

}