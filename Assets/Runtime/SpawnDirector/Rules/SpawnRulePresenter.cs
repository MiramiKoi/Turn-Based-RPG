using Runtime.Common;
using Runtime.Core;
using UnityEngine;

namespace Runtime.SpawnDirector.Rules
{
    public abstract class SpawnRulePresenter : IPresenter
    {
        private readonly SpawnRuleModel _model;
        private readonly World _world;

        protected SpawnRulePresenter(SpawnRuleModel model, World world)
        {
            _model = model;
            _world = world;
        }

        public void Enable()
        {
            _world.TurnBaseModel.OnWorldStepFinished += HandleWorldStepFinished;
            Initialize();
        }

        public void Disable()
        {
            _world.TurnBaseModel.OnWorldStepFinished -= HandleWorldStepFinished;
        }

        protected abstract void Initialize();

        protected void SpawnOne()
        {
            var position = GetSpawnPosition();
            var unit = _world.UnitCollection.Create(_model.Description.UnitDescriptionId);
            unit.Movement.MoveTo(position);
            _world.GridModel.TryPlace(unit, position);
            _model.Units.Add(unit);
        }

        private Vector2Int GetSpawnPosition()
        {
            if (_model.Description.Spawn.Mode == "fixed" && _model.Description.Spawn.FixedPosition.HasValue)
                return _model.Description.Spawn.FixedPosition.Value;

            var position = _world.GridModel.GetRandomAvailablePosition();
            return position ?? Vector2Int.zero;
        }

        protected virtual void HandleWorldStepFinished()
        {
            _model.Units.RemoveAll(unit => unit == null || unit.IsDead);
        }
    }
}