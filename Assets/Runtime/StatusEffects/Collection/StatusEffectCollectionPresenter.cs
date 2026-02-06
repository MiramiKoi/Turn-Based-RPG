using System.Collections.Generic;
using Runtime.Common;
using Runtime.Core;
using Runtime.UI;
using Runtime.Units;
using Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace Runtime.StatusEffects.Collection
{
    public class StatusEffectCollectionPresenter : IPresenter
    {
        private readonly StatusEffectModelCollection _collection;
        private readonly StatusEffectCollectionView _view;
        private readonly UnitModel _unit;
        private readonly World _world;
        private readonly WorldViewDescriptions _viewDescriptions;
        private readonly UIContent _uiContent;
        private readonly Dictionary<string, StatusEffectPresenter> _presenters = new();

        public StatusEffectCollectionPresenter(StatusEffectModelCollection collection, StatusEffectCollectionView view, UnitModel unit, World world, WorldViewDescriptions viewDescriptions, UIContent uiContent)
        {
            _collection = collection;
            _view = view;
            _unit = unit;
            _world = world;
            _viewDescriptions = viewDescriptions;
            _uiContent = uiContent;
        }

        public void Enable()
        {
            _uiContent.GameplayContent.Add(_view.Root);
            _world.TurnBaseModel.OnWorldStepFinished += Tick;
            _collection.OnAdded += HandleAdded;
            _collection.OnRemoved += HandleRemoved;

            foreach (var model in _collection.Models.Values)
            {
                AddPresenter(model);
            }
        }

        public void Disable()
        {
            _uiContent.GameplayContent.Remove(_view.Root);
            _world.TurnBaseModel.OnWorldStepFinished -= Tick;
            _collection.OnAdded -= HandleAdded;
            _collection.OnRemoved -= HandleRemoved;

            foreach (var presenter in _presenters.Values)
            {
                presenter.Disable();
            }

            _presenters.Clear();
        }

        private void Tick()
        {
            var expired = new List<StatusEffectModel>();

            foreach (var pair in _presenters)
            {
                pair.Value.Tick();

                if (pair.Value.IsExpired)
                    expired.Add(_collection.Get(pair.Key));
            }

            foreach (var model in expired)
            {
                _collection.Remove(model.Id);
            }
        }

        private async void AddPresenter(StatusEffectModel model)
        {
            var viewDescription = _viewDescriptions.StatusEffectViewDescriptions.Get(model.Description.ViewId);
            var loadModel = _world.AddressableModel.Load<VisualTreeAsset>(viewDescription.StatusEffectViewAsset.AssetGUID);
            await loadModel.LoadAwaiter;
            
            var view = new StatusEffectView(loadModel.Result);
            _view.Root.Add(view.Root);
            
            var presenter = new StatusEffectPresenter(model, view, _unit, _world, viewDescription);
            presenter.Enable();
            _presenters.Add(model.Id, presenter);
        }

        private void HandleAdded(StatusEffectModel model)
        {
            AddPresenter(model);
        }

        private void HandleRemoved(StatusEffectModel model)
        {
            _presenters.Remove(model.Id);
        }
    }
}