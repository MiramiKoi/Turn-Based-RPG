using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Common;
using Runtime.Core;
using Runtime.TurnBase;

namespace Runtime.LoadSteps
{
    public class TurnBaseLoadStep : IStep
    {
        private readonly World _world;
        private readonly List<IPresenter> _presenters;

        public TurnBaseLoadStep(List<IPresenter> presenters, World world)
        {
            _presenters = presenters;
            _world = world;
        }

        public async Task Run()
        {
            _world.TurnBaseModel.Steps.Clear();

            var turnBasePresenter = new TurnBasePresenter(_world.TurnBaseModel, _world);
            turnBasePresenter.Enable();
            _presenters.Add(turnBasePresenter);

            await Task.CompletedTask;
        }
    }
}