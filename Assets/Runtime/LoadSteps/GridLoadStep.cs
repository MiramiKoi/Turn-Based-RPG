using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Common;
using Runtime.Core;
using Runtime.Landscape.Grid;
using Runtime.Landscape.Grid.Indication;
using Runtime.Landscape.Grid.Interaction;
using Runtime.ViewDescriptions;

namespace Runtime.LoadSteps
{
    public class GridLoadStep : IStep
    {
        private readonly World _world;
        private readonly GridView _gridView;
        private readonly WorldViewDescriptions _viewDescriptions;
        private readonly List<IPresenter> _presenters;

        public GridLoadStep(List<IPresenter> presenters, World world, GridView gridView, WorldViewDescriptions viewDescriptions)
        {
            _world = world;
            _gridView = gridView;
            _viewDescriptions = viewDescriptions;
            _presenters = presenters;
        }

        public Task Run()
        {
            var gridPresenter = new GridPresenter(_world.GridModel, _gridView, _world, _viewDescriptions);
            gridPresenter.Enable();
            _presenters.Add(gridPresenter);

            var gridInteractionPresenter = new GridInteractionPresenter(_world.GridInteractionModel, _gridView, _world);
            gridInteractionPresenter.Enable();
            _presenters.Add(gridInteractionPresenter);

            var gridIndicationPresenter = new GridIndicationPresenter(_gridView, _world, _viewDescriptions);
            gridIndicationPresenter.Enable();
            _presenters.Add(gridIndicationPresenter);

            return Task.CompletedTask;
        }
    }
}