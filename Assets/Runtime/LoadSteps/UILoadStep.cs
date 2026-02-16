using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Common;
using Runtime.Core;
using Runtime.UI;
using Runtime.UI.Blocker;
using Runtime.UI.Loot;
using Runtime.ViewDescriptions;

namespace Runtime.LoadSteps
{
    public class UILoadStep : IStep
    {
        private readonly World _world;
        private readonly WorldViewDescriptions _worldViewDescriptions;
        private readonly List<IPresenter> _presenters;

        public UILoadStep(List<IPresenter> presenters, World world, WorldViewDescriptions worldViewDescriptions)
        {
            _presenters = presenters;
            _world = world;
            _worldViewDescriptions = worldViewDescriptions;
        }

        public async Task Run()
        {
            var uiController = new UIController(_world, _worldViewDescriptions);
            uiController.Enable();
            _presenters.Add(uiController);

            var lootPresenter = new LootPresenter(_world.LootModel, _world, _worldViewDescriptions);
            lootPresenter.Enable();
            _presenters.Add(lootPresenter);

            var uiBlockerPresenter =
                new UIBlockerPresenter(_world.UIBlockerModel, _world, _worldViewDescriptions.UIContent);
            uiBlockerPresenter.Enable();
            _presenters.Add(uiBlockerPresenter);

            await Task.CompletedTask;
        }
    }
}