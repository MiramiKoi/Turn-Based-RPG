using System.Collections.Generic;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Common.ObjectPool;
using Runtime.Core;
using Runtime.Player;
using Runtime.ViewDescriptions;
using UnityEngine;

namespace Runtime.Units.Collection
{
    public class UnitModelCollectionPresenter : IPresenter
    {
        private readonly UnitModelCollection _modelCollection;
        private readonly UnitModelCollectionView _collectionView;
        private readonly WorldViewDescriptions _worldViewDescriptions;
        private readonly World _world;

        private readonly Dictionary<string, (IObjectPool<UnitView>, LoadModel<GameObject>)> _viewPools = new();
        private readonly Dictionary<string, UnitPresenter> _presenters = new();

        public UnitModelCollectionPresenter(UnitModelCollection modelCollection, UnitModelCollectionView collectionView,
            World world, WorldViewDescriptions viewDescriptions)
        {
            _modelCollection = modelCollection;
            _collectionView = collectionView;
            _world = world;
            _worldViewDescriptions = viewDescriptions;
        }

        public async void Enable()
        {
            foreach (var viewDescription in _worldViewDescriptions.UnitViewDescriptions.Descriptions)
            {
                var loadModel = _world.AddressableModel.Load<GameObject>(viewDescription.Prefab.AssetGUID);
                await loadModel.LoadAwaiter;
                var prefab = loadModel.Result.GetComponent<UnitView>();
                _viewPools[viewDescription.Id] = (new ObjectPool<UnitView>(prefab, 10, _collectionView.Transform), loadModel);
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

            _viewPools.Clear();
        }

        private void AddPresenter(UnitModel model)
        {
            var id = model.Id;

            UnitPresenter presenter;

            if (model is PlayerModel playerModel)
            {
                presenter = new PlayerPresenter(playerModel, _viewPools[playerModel.Description.ViewId].Item1, _world,
                    _worldViewDescriptions);
                _world.PlayerModel.Value = playerModel;
            }
            else
            {
                presenter = new UnitPresenter(model, _viewPools[model.Description.ViewId].Item1, _world,
                    _worldViewDescriptions);
            }

            _presenters[id] = presenter;
            presenter.Enable();
        }

        private void RemovePresenter(string id)
        {
            _presenters[id].Disable();
            _presenters.Remove(id);
        }

        private void HandleAdded(UnitModel model)
        {
            AddPresenter(model);
        }

        private void HandleRemoved(UnitModel model)
        {
            RemovePresenter(model.Id);
        }
    }
}