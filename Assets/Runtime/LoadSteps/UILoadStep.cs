using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Common;
using Runtime.Core;
using Runtime.UI;
using Runtime.UI.Loot;
using Runtime.ViewDescriptions;

namespace Runtime.LoadSteps
{
    public class UILoadStep : IStep
    {
        private readonly World _world;
        private readonly WorldViewDescriptions _worldViewDescriptions;
        private readonly UIContent _uiContent;
        private readonly List<IPresenter> _presenters;

        public UILoadStep(List<IPresenter> presenters, World world, WorldViewDescriptions worldViewDescriptions, UIContent uiContent)
        {
            _presenters = presenters;
            _world = world;
            _worldViewDescriptions = worldViewDescriptions;
            _uiContent = uiContent;
        }

        public async Task Run()
        {
            var uiController = new UIController(_world, _world.PlayerControls, _worldViewDescriptions, _uiContent);
            uiController.Enable();
            _presenters.Add(uiController);
            
            var lootPresenter = new LootPresenter(_world.LootModel, _world, _worldViewDescriptions, _uiContent);
            lootPresenter.Enable();
            _presenters.Add(lootPresenter);

            await Task.CompletedTask;
        }
    }
}