using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.CameraControl;
using Runtime.Common;
using Runtime.Core;

namespace Runtime.LoadSteps
{
    public class CameraControlLoadStep : IStep
    {
        private readonly List<IPresenter> _presenters;
        private readonly CameraControlView _view;
        private readonly World _world;

        public CameraControlLoadStep(List<IPresenter> presenters, CameraControlView view, World world)
        {
            _view = view;
            _presenters = presenters;
            _world = world;
        }

        public Task Run()
        {
            var presenter = new CameraControlPresenter(_world.CameraControlModel, _view, _world);
            presenter.Enable();
            _presenters.Add(presenter);
            return Task.CompletedTask;
        }
    }
}