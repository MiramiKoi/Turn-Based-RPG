using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Common;
using Runtime.Core;
using Runtime.UI;
using Runtime.UI.Player.StatusEffects;
using Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.LoadSteps
{
    public class PlayerLoadStep : IStep
    {
        private readonly List<IPresenter> _presenters;
        private readonly World _world;
        private readonly WorldViewDescriptions _worldViewDescriptions;
        private readonly UIContent _uiContent;

        public PlayerLoadStep(List<IPresenter> presenters, World world, WorldViewDescriptions worldViewDescriptions, UIContent uiContent)
        {
            _presenters = presenters;
            _world = world;
            _worldViewDescriptions = worldViewDescriptions;
            _uiContent = uiContent;
        }

        public async Task Run()
        {
            var characterModel = _world.UnitCollection.Create("character");
            characterModel.MoveTo(new Vector2Int(50, 50));
            
            _world.GridModel.TryPlace(characterModel, characterModel.Position.Value);
            
            var loadModelUiAsset = _world.AddressableModel.Load<VisualTreeAsset>(_worldViewDescriptions
                .StatusEffectViewDescriptions.StatusEffectContainerAsset.AssetGUID);
            await loadModelUiAsset.LoadAwaiter;
            var statusEffectsView = new PlayerStatusEffectHudView(loadModelUiAsset.Result);
            var statusEffectsPresenter = new PlayerStatusEffectsHudPresenter(characterModel, statusEffectsView, _world,
                _worldViewDescriptions, _uiContent);
            statusEffectsPresenter.Enable();
            _presenters.Add(statusEffectsPresenter);

            await Task.CompletedTask;
        }
    }
}