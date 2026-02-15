using Runtime.Core;

namespace Runtime.SpawnDirector.Rules
{
    public class SoloSpawnRulePresenter : SpawnRulePresenter
    {
        private readonly SpawnRuleModel _model;

        public SoloSpawnRulePresenter(SpawnRuleModel model, World world) : base(model, world)
        {
            _model = model;
        }

        protected override void Initialize()
        {
            if (_model.AliveCount() == 0)
            {
                SpawnOne();
            }
        }

        protected override void HandleWorldStepFinished()
        {
            base.HandleWorldStepFinished();
            var alive = _model.AliveCount();

            if (alive > 0)
            {
                _model.RespawnCounter = 0;
                return;
            }

            if (_model.Description.Respawn.Enabled)
            {
                _model.RespawnCounter++;
                if (_model.RespawnCounter >= _model.Description.Respawn.DelaySteps)
                {
                    SpawnOne();
                    _model.RespawnCounter = 0;
                }
            }
            else
            {
                if (alive == 0 && _model.Units.Count == 0)
                    SpawnOne();
            }
        }
    }
}