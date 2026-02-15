using Runtime.Common;
using Runtime.Core;
using Runtime.SpawnDirector.Corpse;
using UnityEngine;

namespace Runtime.SpawnDirector.Rules
{
    public abstract class SpawnRulePresenter : IPresenter
    {
        private readonly SpawnRuleModel _model;
        private readonly World _world;

        private CorpseRulePresenter _corpseRulePresenter;

        protected SpawnRulePresenter(SpawnRuleModel model, World world)
        {
            _model = model;
            _world = world;
        }

        public void Enable()
        {
            _world.TurnBaseModel.OnWorldStepFinished += HandleStep;
            Initialize();
        }

        public void Disable()
        {
            _world.TurnBaseModel.OnWorldStepFinished -= HandleStep;
            _corpseRulePresenter?.Disable();
            _corpseRulePresenter = null;
        }

        protected virtual void Initialize()
        {
            if (!_model.Description.Corpse.IsInfinite)
            {
                var corpseModel = new CorpseRuleModel(_model, _model.Description.Corpse);
                _corpseRulePresenter = new CorpseRulePresenter(corpseModel, _world);
                _corpseRulePresenter.Enable();
            }
        }

        protected void SpawnOne()
        {
            var position = GetSpawnPosition();
            var unit = _world.UnitCollection.Create(_model.Description.UnitDescriptionId);
            unit.Movement.SetPosition(position);
            _model.Units.Add(unit);
        }

        private Vector2Int GetSpawnPosition()
        {
            if (_model.Description.Spawn.Mode == "fixed" && _model.Description.Spawn.FixedPosition.HasValue)
                return _model.Description.Spawn.FixedPosition.Value;

            var position = _world.GridModel.GetRandomAvailablePosition();
            return position ?? Vector2Int.zero;
        }

        protected abstract void HandleStep();
    }
}