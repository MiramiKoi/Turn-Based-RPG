using System.Collections.Generic;
using System.Linq;
using Runtime.Common;
using Runtime.Core;
using Runtime.Units;

namespace Runtime.SpawnDirector.Corpse
{
    public sealed class CorpseRulePresenter : IPresenter
    {
        private readonly CorpseRuleModel _model;
        private readonly World _world;

        public CorpseRulePresenter(CorpseRuleModel model, World world)
        {
            _model = model;
            _world = world;
        }

        public void Enable()
        {
            _world.TurnBaseModel.OnWorldStepFinished += HandleStep;
        }

        public void Disable()
        {
            _world.TurnBaseModel.OnWorldStepFinished -= HandleStep;
        }

        private void HandleStep()
        {
            foreach (var unit in _model.SpawnRuleModel.Units.Where(unit =>
                         unit is { IsDead: true } && !_model.CorpseCounters.ContainsKey(unit)))
            {
                _model.RegisterCorpse(unit);
            }

            var toRemove = new List<UnitModel>();
            foreach (var unitModel in _model.CorpseCounters.Keys.ToList())
            {
                _model.CorpseCounters[unitModel]--;

                if (_model.CorpseCounters[unitModel] <= 0)
                    toRemove.Add(unitModel);
            }

            foreach (var unit in toRemove)
            {
                RemoveCorpse(unit);
            }
        }

        private void RemoveCorpse(UnitModel unit)
        {
            _model.CorpseCounters.Remove(unit);
            _world.UnitCollection.Remove(unit.Id);
            _model.SpawnRuleModel.Units.Remove(unit);
        }
    }
}