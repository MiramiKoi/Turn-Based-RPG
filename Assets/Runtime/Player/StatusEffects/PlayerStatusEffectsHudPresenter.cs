using System.Collections.Generic;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Core;
using Runtime.StatusEffects;
using Runtime.StatusEffects.Collection;
using Runtime.UI;
using Runtime.Units;
using Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace Runtime.Player.StatusEffects
{
    public class PlayerStatusEffectsHudPresenter : IPresenter
    {
        private readonly StatusEffectModelCollection _modelCollection;
        private readonly PlayerStatusEffectHudView _view;
        private readonly World _world;
        private readonly WorldViewDescriptions _viewDescriptions;
        private readonly UIContent _uiContent;

        private readonly Dictionary<string, StatusEffectPresenter> _presenters = new();
        private readonly Dictionary<string, StatusEffectView> _views = new();
        private readonly Dictionary<string, LoadModel<VisualTreeAsset>> _loadModels = new();

        public PlayerStatusEffectsHudPresenter(UnitModel unit, PlayerStatusEffectHudView view, World world, WorldViewDescriptions viewDescriptions, UIContent uiContent)
        {
            _modelCollection = unit.ActiveEffects;
            _view = view;
            _world = world;
            _viewDescriptions = viewDescriptions;
            _uiContent = uiContent;
        }

        public void Enable()
        {
            _uiContent.GameplayContent.Add(_view.Root);
            _modelCollection.OnAdded += HandleAdded;
            _modelCollection.OnRemoved += HandleRemoved;

            foreach (var model in _modelCollection.Models.Values)
            {
                AddPresenter(model);
            }
        }

        public void Disable()
        {
            _uiContent.GameplayContent.Remove(_view.Root);
            _modelCollection.OnAdded -= HandleAdded;
            _modelCollection.OnRemoved -= HandleRemoved;

            var ids = new List<string>(_loadModels.Keys);
            foreach (var id in ids)
            {
                RemovePresenter(id);
            }

            _presenters.Clear();
            _views.Clear();
            _loadModels.Clear();
        }

        private async void AddPresenter(StatusEffectModel model)
        {
            var id = model.Id;

            var viewDescription = _viewDescriptions.StatusEffectViewDescriptions.Get(model.Description.ViewId);

            var loadModel = _world.AddressableModel.Load<VisualTreeAsset>(
                _viewDescriptions.StatusEffectViewDescriptions.StatusEffectViewAsset.AssetGUID);

            _loadModels[id] = loadModel;

            await loadModel.LoadAwaiter;

            var view = new StatusEffectView(loadModel.Result);
            _views[id] = view;
            _view.Root.Add(view.Root);

            var presenter = new StatusEffectPresenter(model, view, _world, viewDescription);
            _presenters[id] = presenter;
            presenter.Enable();
        }

        private void RemovePresenter(string id)
        {
            _presenters[id].Disable();
            _presenters.Remove(id);

            _views[id].Root.RemoveFromHierarchy();
            _views.Remove(id);

            _world.AddressableModel.Unload(_loadModels[id]);
            _loadModels.Remove(id);
        }

        private void HandleAdded(StatusEffectModel model)
        {
            AddPresenter(model);
        }

        private void HandleRemoved(StatusEffectModel model)
        {
            RemovePresenter(model.Id);
        }
    }
}