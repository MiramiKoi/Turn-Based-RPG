using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Common;
using Runtime.Core;
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

        public PlayerLoadStep(List<IPresenter> presenters, World world, WorldViewDescriptions worldViewDescriptions)
        {
            _presenters = presenters;
            _world = world;
            _worldViewDescriptions = worldViewDescriptions;
        }

        public async Task Run()
        {
            var characterModel = _world.UnitCollection.Create("character");
            characterModel.Movement.MoveTo(new Vector2Int(50, 50));

            _world.GridModel.TryPlace(characterModel, characterModel.State.Position.Value);

            var loadModelUiAsset = _world.AddressableModel.Load<VisualTreeAsset>(_worldViewDescriptions
                .StatusEffectViewDescriptions.StatusEffectContainerAsset.AssetGUID);
            await loadModelUiAsset.LoadAwaiter;
            var statusEffectsView = new PlayerStatusEffectHudView(loadModelUiAsset.Result);
            var statusEffectsPresenter = new PlayerStatusEffectsHudPresenter(characterModel, statusEffectsView, _world,
                _worldViewDescriptions);
            statusEffectsPresenter.Enable();
            _presenters.Add(statusEffectsPresenter);

            await Task.CompletedTask;
        }
    }
}