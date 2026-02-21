using System.Collections.Generic;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Common.ObjectPool;
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
        private readonly StatusEffectCollectionView _collectionView;
        private readonly StatusEffectViewDescriptionCollection _viewDescriptions;
        private readonly UnitModel _unitModel;
        private readonly World _world;

        private readonly Dictionary<string, (IObjectPool<StatusEffectView>, LoadModel<GameObject>)> _viewPools = new();
        private readonly Dictionary<string, StatusEffectPresenter> _presenters = new();

        public StatusEffectCollectionPresenter(UnitModel unitModel, UnitView unitView, World world,
            WorldViewDescriptions viewDescriptions)
        {
            _modelCollection = unitModel.Effects.Collection;
            _collectionView = unitView.StatusEffectCollectionView;
            _unitModel = unitModel;
            _world = world;
            _viewDescriptions = viewDescriptions.StatusEffectViewDescriptions;
        }

        public async void Enable()
        {
            foreach (var viewDescription in _viewDescriptions.Descriptions)
            {
                var loadModel = _world.AddressableModel.Load<GameObject>(viewDescription.View.AssetGUID);
                await loadModel.LoadAwaiter;
                var prefab = loadModel.Result.GetComponent<StatusEffectView>();
                _viewPools[viewDescription.Id] = (new ObjectPool<StatusEffectView>(prefab, 5, _collectionView.Transform), loadModel);
            }

            _modelCollection.OnAdded += HandleAdded;
            _modelCollection.OnRemoved += HandleRemoved;

            foreach (var model in _modelCollection.Models.Values)
            {
                AddPresenter(model);
            }
        }

        public void Disable()
        {
            _modelCollection.OnAdded -= HandleAdded;
            _modelCollection.OnRemoved -= HandleRemoved;

            foreach (var presenter in _presenters.Values)
            {
                presenter.Disable();
            }

            _presenters.Clear();

            foreach (var (pool, loadModel) in _viewPools.Values)
            {
                pool.Dispose();
                _world.AddressableModel.Unload(loadModel);
            }
        }

        private void AddPresenter(StatusEffectModel model)
        {
            model.OnExpired += HandleChangeExpired;

            var id = model.Id;

            var presenter = new StatusEffectPresenter(model, _viewPools[model.Description.ViewId].Item1, _unitModel, _world);
            _presenters[id] = presenter;
            presenter.Enable();
        }

        private void RemovePresenter(string id)
        {
            _presenters[id].Disable();
            _presenters.Remove(id);
        }

        private void HandleChangeExpired(StatusEffectModel statusEffectModel)
        {
            _modelCollection.Remove(statusEffectModel.Id);
        }

        private void HandleAdded(StatusEffectModel model)
        {
            AddPresenter(model);
        }

        private void HandleRemoved(StatusEffectModel model)
        {
            model.OnExpired -= HandleChangeExpired;
            RemovePresenter(model.Id);
        }
    }
}