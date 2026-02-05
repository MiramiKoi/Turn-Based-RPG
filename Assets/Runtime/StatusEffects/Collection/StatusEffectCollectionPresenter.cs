using System.Collections.Generic;
using Runtime.Common;
using Runtime.Core;
using Runtime.Descriptions.StatusEffects.Enums;
using Runtime.Units;

namespace Runtime.StatusEffects.Collection
{
    public class StatusEffectCollectionPresenter : IPresenter
    {
        private readonly StatusEffectModelCollection _collection;
        private readonly UnitModel _unit;
        private readonly World _world;
        private readonly Dictionary<string, StatusEffectPresenter> _presenters = new();

        public StatusEffectCollectionPresenter(StatusEffectModelCollection collection, UnitModel unit, World world)
        {
            _collection = collection;
            _unit = unit;
            _world = world;
        }

        public void Enable()
        {
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
                pair.Value.Tick(TickMoment.TurnEnd);

                if (pair.Value.IsExpired)
                    expired.Add(_collection.Get(pair.Key));
            }

            foreach (var model in expired)
            {
                _collection.Remove(model.Id);
            }
        }

        private void AddPresenter(StatusEffectModel model)
        {
            var presenter = new StatusEffectPresenter(model, _unit, _world);
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