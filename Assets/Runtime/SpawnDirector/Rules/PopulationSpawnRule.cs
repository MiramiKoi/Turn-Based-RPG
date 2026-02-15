using Runtime.Core;

namespace Runtime.SpawnDirector.Rules
{
    public class PopulationSpawnRule : SpawnRulePresenter
    {
        private readonly SpawnRuleModel _model;

        public PopulationSpawnRule(SpawnRuleModel model, World world) : base(model, world)
        {
            _model = model;
        }

        protected override void Initialize()
        {
            var target = _model.Description.Count ?? 1;
            var alive = _model.AliveCount();
            var toSpawn = target - alive;

            for (var i = 0; i < toSpawn; i++)
                SpawnOne();
        }

        protected override void HandleWorldStepFinished()
        {
            base.HandleWorldStepFinished();
            var alive = _model.AliveCount();
            
            if (_model.Description.RuleType == "population")
            {
                var target = _model.Description.Count ?? 1;
                var toSpawn = target - alive;
                if (toSpawn <= 0)
                {
                    _model.RespawnCounter = 0;
                    return;
                }

                if (_model.Description.Respawn.Enabled)
                {
                    _model.RespawnCounter++;
                    if (_model.RespawnCounter >= _model.Description.Respawn.DelaySteps)
                    {
                        for (var i = 0; i < toSpawn; i++)
                            SpawnOne();
                        _model.RespawnCounter = 0;
                    }
                }
                else
                {
                    for (var i = 0; i < toSpawn; i++)
                        SpawnOne();
                }
            }
        }
    }
}