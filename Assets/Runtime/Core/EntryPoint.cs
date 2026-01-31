using System.Collections.Generic;
using System.Threading.Tasks;
using fastJSON;
using Runtime.Agents;
using Runtime.Agents.Nodes;
using Runtime.AsyncLoad;
using Runtime.CameraControl;
using Runtime.Common;
using Runtime.Descriptions;
using Runtime.Input;
using Runtime.Landscape.Grid;
using Runtime.LoadSteps;
using Runtime.Player;
using Runtime.Units;
using Runtime.ViewDescriptions;
using UnityEngine;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private CameraControlView _cameraControlView;
        [SerializeField] private GridView _gridView;
        
        private readonly World _world = new();
        private readonly WorldDescription _worldDescription = new();
        private readonly WorldViewDescriptions _worldViewDescriptions = new();
        
        private readonly AddressableModel _addressableModel = new();
        private readonly List<IPresenter> _presenters = new();

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
                new CameraControlLoadStep(_presenters, _cameraControlView, _world)
            };

            foreach (var step in persistentLoadStep)
            {
                await step.Run();
            }
            
            await CreateControllableUnit();
            await CreateUnit();
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
            
            var playerPresenter = new PlayerPresenter(unitModel, unitView, _world);
            playerPresenter.Enable();
        }
        
        
        private async Task CreateUnit()
        {
            var unitModel = _world.UnitCollection.Get("bear");
            
            var unitViewDescription = _worldViewDescriptions.UnitViewDescriptions.Get(unitModel.Description.ViewId);
            var loadModel = _addressableModel.Load<GameObject>(unitViewDescription.Prefab.AssetGUID);
            await loadModel.LoadAwaiter;
            var unitPrefab = loadModel.Result;
            var unitView = Instantiate(unitPrefab.GetComponent<UnitView>(), Vector3.zero, Quaternion.identity);
            _addressableModel.Unload(loadModel);
            
            var dictionary = JSON.ToObject<Dictionary<string, object>>(Resources.Load<TextAsset>("unit").text);

            var decisionRoot = new AgentDecisionRoot();
            
            decisionRoot.Deserialize(dictionary);
            
            var playerPresenter = new AgentPresenter(unitModel, decisionRoot, _world);

            var unitPresenter = new UnitPresenter(unitModel, unitView);
            
            unitModel.RegisterCommand("move_right", new MoveCommand(Vector2Int.right));
            unitModel.RegisterCommand("move_left", new MoveCommand(Vector2Int.left));
            unitModel.RegisterCommand("set_false_flag", new SetFlagCommand(false, "flag"));
            unitModel.RegisterCommand("set_true_flag", new SetFlagCommand(true, "flag"));
            unitModel.RegisterCommand("has_flag", new HasFlagCommand("flag"));
            
            
            unitPresenter.Enable();
            playerPresenter.Enable();
        }
    }
}