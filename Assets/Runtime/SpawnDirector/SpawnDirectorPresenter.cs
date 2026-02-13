using Runtime.Common;
using Runtime.Core;

namespace Runtime.SpawnDirector
{
    public class SpawnDirectorPresenter : IPresenter
    {
        private readonly SpawnDirectorModel _model;
        private readonly World _world;

        public SpawnDirectorPresenter(SpawnDirectorModel model, World world)
        {
            _model = model;
            _world = world;
        }

        public void Enable()
        {
            _world.TurnBaseModel.OnWorldStepFinished += HandleTick;
        }

        public void Disable()
        {
            _world.TurnBaseModel.OnWorldStepFinished -= HandleTick;
        }

        private void HandleTick()
        {
            foreach (var modelRule in _model.Rules)
            {
                modelRule.Run();
            }
        }
    }
}
