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

        private readonly Dictionary<string, StatusEffectPresenter> _systems = new();

        public StatusEffectCollectionPresenter(UnitModel unit, World world)
        {
            _modelCollection = unit.ActiveEffects;
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
                AddSystem(model);
            }
        }

        public void Disable()
        {
            _world.TurnBaseModel.OnWorldStepFinished -= Tick;
            _modelCollection.OnAdded -= HandleAdded;
            _modelCollection.OnRemoved -= HandleRemoved;

            foreach (var pair in _systems)
            {
                pair.Value.Disable();
            }
            _systems.Clear();
        }

        private void Tick()
        {
            _unit.ResetActionDisables();

            var expired = new List<StatusEffectModel>();

            foreach (var pair in _systems)
            {
                pair.Value.Tick();

                if (pair.Value.IsExpired)
                    expired.Add(_modelCollection.Get(pair.Key));
            }

            foreach (var model in expired)
            {
                _modelCollection.Remove(model.Id);
            }
        }

        private void AddSystem(StatusEffectModel model)
        {
            var id = model.Id;
            var presenter = new StatusEffectPresenter(model, _unit, _world);
            _systems[id] = presenter;
            presenter.Enable();
        }

        private void RemoveSystem(string id)
        {
            _systems[id].Disable();
            _systems.Remove(id);
        }

        private void HandleAdded(StatusEffectModel model)
        {
            AddSystem(model);
        }

        private void HandleRemoved(StatusEffectModel model)
        {
            RemoveSystem(model.Id);
        }
    }
}