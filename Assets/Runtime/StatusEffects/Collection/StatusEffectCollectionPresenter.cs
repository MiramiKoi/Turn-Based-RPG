using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.Units;
using Runtime.ViewDescriptions;
using Runtime.ViewDescriptions.StatusEffects;
using UnityEngine;

namespace Runtime.StatusEffects.Collection
{
    public class StatusEffectCollectionPresenter : IPresenter
    {
        private readonly StatusEffectModelCollection _modelCollection;
        private readonly StatusEffectViewDescriptionCollection _viewDescriptions;
        private readonly UnitView _unitView;
        private readonly UnitModel _unitModel;
        private readonly World _world;

        private readonly Dictionary<string, (StatusEffectPresenter, LoadModel<GameObject>)> _presenters = new();

        public StatusEffectCollectionPresenter(UnitModel unitModel, UnitView unitUnitView, World world, WorldViewDescriptions viewDescriptions)
        {
            _modelCollection = unitModel.ActiveEffects.Collection;
            _unitModel = unitModel;
            _unitView = unitUnitView;
            _world = world;
            _viewDescriptions = viewDescriptions.StatusEffectViewDescriptions;
        }

        public async void Enable()
        {
            _modelCollection.OnAdded += HandleAdded;
            _modelCollection.OnRemoved += HandleRemoved;

            foreach (var model in _modelCollection.Models.Values)
            {
                await AddPresenter(model);
            }
        }

        public void Disable()
        {
            _modelCollection.OnAdded -= HandleAdded;
            _modelCollection.OnRemoved -= HandleRemoved;

            foreach (var (presenter, loadModel) in _presenters.Values)
            {
                presenter.Disable();
                _world.AddressableModel.Unload(loadModel);
            }

            _presenters.Clear();
        }

        private async Task AddPresenter(StatusEffectModel model)
        {
            model.OnExpired += HandleChangeExpired;

            var id = model.Id;
            
            var viewDescription = _viewDescriptions.Get(model.Description.ViewId);
            var loadModel = _world.AddressableModel.Load<GameObject>(viewDescription.View.AssetGUID);
            
            await loadModel.LoadAwaiter;
            
            var viewPrefab = loadModel.Result.GetComponent<StatusEffectView>();

            var view = (await Object.InstantiateAsync(viewPrefab, _unitView.Transform))[0];
            view.Transform.position = _unitView.Transform.position;

            var presenter = new StatusEffectPresenter(model, view, _unitModel, _world);
            _presenters[id] = (presenter, loadModel);
            presenter.Enable();
        }

        private void RemovePresenter(string id)
        {
            _presenters[id].Item1.Disable();
            _world.AddressableModel.Unload(_presenters[id].Item2);
            _presenters.Remove(id);
        }

        private void HandleChangeExpired(StatusEffectModel statusEffectModel)
        {
            _modelCollection.Remove(statusEffectModel.Id);
        }

        private async void HandleAdded(StatusEffectModel model)
        { 
            await AddPresenter(model);
        }

        private void HandleRemoved(StatusEffectModel model)
        {
            model.OnExpired -= HandleChangeExpired;
            RemovePresenter(model.Id);
        }
    }
}