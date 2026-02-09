using System.Collections.Generic;
using Runtime.Common;
using Runtime.Core;
using Runtime.Units;

namespace Runtime.StatusEffects.Collection
{
    public class StatusEffectCollectionPresenter : IPresenter
    {
        private readonly StatusEffectModelCollection _modelCollection;
        private readonly UnitModel _unit;
        private readonly World _world;

        private readonly Dictionary<string, StatusEffectPresenter> _presenters = new();

        public StatusEffectCollectionPresenter(UnitModel unit, World world)
        {
            _modelCollection = unit.ActiveEffects.Collection;
            _unit = unit;
            _world = world;
        }

        public void Enable()
        {
            _world.TurnBaseModel.OnWorldStepFinished += Tick;
            _modelCollection.OnAdded += HandleAdded;
            _modelCollection.OnRemoved += HandleRemoved;

            foreach (var model in _modelCollection.Models.Values)
            {
                AddPresenter(model);
            }
        }

        public void Disable()
        {
            _world.TurnBaseModel.OnWorldStepFinished -= Tick;
            _modelCollection.OnAdded -= HandleAdded;
            _modelCollection.OnRemoved -= HandleRemoved;

            foreach (var presenter in _presenters.Values)
            {
                presenter.Disable();
            }
            _presenters.Clear();
        }

        private void Tick()
        {
            _unit.ResetActionDisables();

            var expired = new List<StatusEffectModel>();

            foreach (var presenter in _presenters)
            {
                presenter.Value.Tick();

                if (presenter.Value.IsExpired)
                    expired.Add(_modelCollection.Get(presenter.Key));
            }

            foreach (var model in expired)
            {
                _modelCollection.Remove(model.Id);
            }
        }

        private void AddPresenter(StatusEffectModel model)
        {
            var id = model.Id;
            var presenter = new StatusEffectPresenter(model, _unit, _world);
            _presenters[id] = presenter;
            presenter.Enable();
        }

        private void RemoveSystem(string id)
        {
            _presenters[id].Disable();
            _presenters.Remove(id);
        }

        private void HandleAdded(StatusEffectModel model)
        {
            AddPresenter(model);
        }

        private void HandleRemoved(StatusEffectModel model)
        {
            RemoveSystem(model.Id);
        }
    }
}