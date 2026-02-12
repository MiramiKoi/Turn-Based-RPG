using System.Collections.Generic;
using System.Threading.Tasks;
using Runtime.Agents;
using Runtime.AsyncLoad;
using Runtime.CameraControl;
using Runtime.Common;
using Runtime.Descriptions;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Input;
using Runtime.Items.Transfer;
using Runtime.Landscape.Grid;
using Runtime.LoadSteps;
using Runtime.Player;
using Runtime.TurnBase;
using Runtime.UI;
using Runtime.UI.Loot;
using Runtime.UI.Player.StatusEffects;
using Runtime.Units;
using Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private CameraControlView _cameraControlView;
        [SerializeField] private GridView _gridView;
        [SerializeField] private UIDocument _gameplayDocument;

        private readonly World _world = new();
        private readonly WorldDescription _worldDescription = new();
        private readonly WorldViewDescriptions _worldViewDescriptions = new();

        private readonly AddressableModel _addressableModel = new();
        private readonly List<IPresenter> _presenters = new();

        private UIController _uiController;
        private UIContent _uiContent;
        private UIBlocker _uiBlocker;

        private PlayerControls _playerControls;

        private async void Start()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();

            _uiContent = new UIContent(_gameplayDocument);

            IStep[] persistentLoadStep =
            {
                new AddressableLoadStep(_addressableModel, _presenters),
                new DescriptionsLoadStep(_worldDescription, _addressableModel),
                new LuaRuntimeLoadStep(_addressableModel, _worldDescription),
                new ViewDescriptionsLoadStep(_worldViewDescriptions, _addressableModel),
                new WorldLoadStep(_world, _addressableModel, _playerControls, _worldDescription,
                    _uiContent.GameplayContent),
                new GridLoadStep(_presenters, _world, _gridView, _worldViewDescriptions),
                new UnitsLoadStep(_world),
                new CameraControlLoadStep(_presenters, _cameraControlView, _world)
            };

            foreach (var step in persistentLoadStep)
            {
                await step.Run();
            }

            await CreateControllableUnit();

            await CreateUnit("bear_0", _worldDescription.BearAgentDecisionDescription);
            await CreateUnit("panda_0", _worldDescription.PandaAgentDecisionDescription);
            await CreateUnit("trader_0", _worldDescription.TraderAgentDecisionDescription);

            _uiController = new UIController(_world, _playerControls, _worldViewDescriptions, _uiContent);
            _uiController.Enable();

            _world.TurnBaseModel.Steps.Clear();
            var turnBasePresenter = new TurnBasePresenter(_world.TurnBaseModel, _world);

            turnBasePresenter.Enable();

            var lootPresenter = new LootPresenter(_world, _uiContent, _worldViewDescriptions);
            lootPresenter.Enable();

            var transferPresenter = new TransferPresenter(_world.TransferModel);
            transferPresenter.Enable();
        }

        private void Update()
        {
            _world.GameSystems?.Update(Time.deltaTime);
        }

        private async Task CreateControllableUnit()
        {
            var unitModel = (PlayerModel)_world.UnitCollection.Get("character");

            var unitViewDescription = _worldViewDescriptions.UnitViewDescriptions.Get(unitModel.Description.ViewId);
            var loadModelPrefab = _addressableModel.Load<GameObject>(unitViewDescription.Prefab.AssetGUID);
            await loadModelPrefab.LoadAwaiter;
            var unitPrefab = loadModelPrefab.Result.GetComponent<UnitView>();
            var unitView =
                (await InstantiateAsync(unitPrefab, (Vector2)unitModel.Position.Value, Quaternion.identity))[0];
            _world.CameraControlModel.Target.Value = unitView.Transform;
            _addressableModel.Unload(loadModelPrefab);

            var loadModelUiAsset = _addressableModel.Load<VisualTreeAsset>(_worldViewDescriptions
                .StatusEffectViewDescriptions.StatusEffectContainerAsset.AssetGUID);
            await loadModelUiAsset.LoadAwaiter;
            var statusEffectsView = new PlayerStatusEffectHudView(loadModelUiAsset.Result);
            var statusEffectsPresenter = new PlayerStatusEffectsHudPresenter(unitModel, statusEffectsView, _world,
                _worldViewDescriptions, _uiContent);

            var playerPresenter = new PlayerPresenter(unitModel, unitView, _world, _worldViewDescriptions);

            playerPresenter.Enable();
            statusEffectsPresenter.Enable();

            unitModel.ActiveEffects.TryApply("burn");
        }

        private async Task CreateUnit(string id, AgentDecisionDescription description)
        {
            var unitModel = _world.UnitCollection.Get(id);

            var unitViewDescription = _worldViewDescriptions.UnitViewDescriptions.Get(unitModel.Description.ViewId);
            var loadModel = _addressableModel.Load<GameObject>(unitViewDescription.Prefab.AssetGUID);
            await loadModel.LoadAwaiter;
            var unitPrefab = loadModel.Result.GetComponent<UnitView>();
            var unitView =
                (await InstantiateAsync(unitPrefab, (Vector2)unitModel.Position.Value, Quaternion.identity))[0];
            _addressableModel.Unload(loadModel);

            //var agentPresenter = new AgentPresenter(unitModel, _worldDescription.BearAgentDecisionDescription, _world);

            var agentModel = new AgentModel(unitModel, description, _world);
            _world.AgentCollection.Add(unitModel.Id, agentModel);

            var unitPresenter = new UnitPresenter(unitModel, unitView, _world, _worldViewDescriptions);

            unitPresenter.Enable();
            //agentPresenter.Enable();
        }
    }
}