using System.Collections.Generic;
using System.Threading.Tasks;
using Editor.Agents;
using fastJSON;
using Runtime.Agents;
using Runtime.AsyncLoad;
using Runtime.CameraControl;
using Runtime.Common;
using Runtime.Descriptions;
using Runtime.Descriptions.Agents.Nodes;
using Runtime.Input;
using Runtime.Items;
using Runtime.Landscape.Grid;
using Runtime.LoadSteps;
using Runtime.Player;
using Runtime.TurnBase;
using Runtime.UI;
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
            
        private PlayerControls _playerControls;
        
        private async void Start()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();
            
            IStep[] persistentLoadStep =
            {
                new AddressableLoadStep(_addressableModel, _presenters),
                new DescriptionsLoadStep(_worldDescription, _addressableModel),
                new ViewDescriptionsLoadStep(_worldViewDescriptions, _addressableModel),
                new WorldLoadStep(_world, _addressableModel, _playerControls, _worldDescription),
                new GridLoadStep(_presenters, _world, _gridView, _worldViewDescriptions),
                new UnitsLoadStep(_world),
                new CameraControlLoadStep(_presenters, _cameraControlView, _world)
            };

            foreach (var step in persistentLoadStep)
            {
                await step.Run();
            }
            
            await CreateControllableUnit();
            await CreateUnit("bear_0");
            await CreateUnit("bear_1");

            _world.TurnBaseModel.Steps.Clear();
            var turnBasePresenter = new TurnBasePresenter(_world.TurnBaseModel, _world);
            turnBasePresenter.Enable();
            
            _uiContent = new UIContent(_gameplayDocument);
            _uiController = new UIController(_world, _playerControls, _worldViewDescriptions, _uiContent);
            _uiController.Enable();
            
            //TODO: Putting item test
            var itemFactory = new ItemFactory(_world.WorldDescription.ItemCollection);
            _world.InventoryModel.TryPutItem(itemFactory.Create("bear_meat").Description, 14);
            _world.InventoryModel.TryPutItem(itemFactory.Create("bear_fur").Description, 28);
        }
        
        private void Update()
        {
            _world.GameSystems?.Update(Time.deltaTime);
        }

        private async Task CreateControllableUnit()
        {
            var unitModel = _world.UnitCollection.Get("character");
            
            var unitViewDescription = _worldViewDescriptions.UnitViewDescriptions.Get(unitModel.Description.ViewId);
            var loadModel = _addressableModel.Load<GameObject>(unitViewDescription.Prefab.AssetGUID);
            await loadModel.LoadAwaiter;
            var unitPrefab = loadModel.Result;
            var unitView = Instantiate(unitPrefab.GetComponent<UnitView>(), Vector3.zero, Quaternion.identity);
            _world.CameraControlModel.Target.Value = unitView.Transform;
            _addressableModel.Unload(loadModel);
            
            var unitPresenter = new UnitPresenter(unitModel, unitView, _world);

            var playerModel = new PlayerModel(unitModel, _world.GridModel);
            var playerPresenter = new PlayerPresenter(playerModel, _world);
            
            unitPresenter.Enable();
            playerPresenter.Enable();
        }
        
        
        private async Task CreateUnit(string id)
        {
            var unitModel = _world.UnitCollection.Get(id);
            
            var unitViewDescription = _worldViewDescriptions.UnitViewDescriptions.Get(unitModel.Description.ViewId);
            var loadModel = _addressableModel.Load<GameObject>(unitViewDescription.Prefab.AssetGUID);
            await loadModel.LoadAwaiter;
            var unitPrefab = loadModel.Result;
            var unitView = Instantiate(unitPrefab.GetComponent<UnitView>(), Vector3.zero, Quaternion.identity);
            _addressableModel.Unload(loadModel);

            var json = Resources.Load<TextAsset>("bear-description").text;

            var agentDecisionRoot = new AgentDecisionRoot();
            
            agentDecisionRoot.Deserialize(JSON.ToObject<Dictionary<string, object>>(json));
            
            var agentPresenter = new AgentPresenter(unitModel, agentDecisionRoot, _world);
            
            var unitPresenter = new UnitPresenter(unitModel, unitView, _world);
            
            unitPresenter.Enable();
            agentPresenter.Enable();
        }
    }
}