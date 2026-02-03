using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fastJSON;
using Runtime.Agents;
using Runtime.Agents.Nodes;
using Runtime.AsyncLoad;
using Runtime.Common;
using Runtime.Descriptions;
using Runtime.Input;
using Runtime.Landscape.Grid;
using Runtime.Landscape.Grid.Indication;
using Runtime.Landscape.Grid.Interaction;
using Runtime.LoadSteps;
using Runtime.Player;
using Runtime.Units;
using Runtime.ViewDescriptions;
using UnityEngine;

namespace Runtime.Core
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private GridView _gridView;
        
        private readonly World _world = new();
        private readonly WorldDescription _worldDescription = new();
        private readonly WorldViewDescriptions _worldViewDescriptions = new();
        
        private readonly AddressableModel _addressableModel = new();
        private readonly List<IPresenter> _presenters = new();

        private PlayerControls _playerControls;
        
        private async void Start()
        {
            IStep[] persistentLoadStep =
            {
                new AddressableLoadStep(_addressableModel, _presenters),
                new DescriptionsLoadStep(_worldDescription, _addressableModel),
                new ViewDescriptionsLoadStep(_worldViewDescriptions, _addressableModel)
            };

            foreach (var step in persistentLoadStep)
            {
                await step.Run();
            }

            _playerControls = new PlayerControls();
            _playerControls.Enable();
            
            _world.SetData(_addressableModel, _playerControls, _worldDescription);
            
            var gridPresenter = new GridPresenter(_world.GridModel, _gridView, _worldViewDescriptions);
            gridPresenter.Enable();
            
            var gridInteractionPresenter = new GridInteractionPresenter(_world.GridInteractionModel, _gridView, _world);
            gridInteractionPresenter.Enable();
            
            var gridIndicationPresenter = new GridIndicationPresenter(_gridView, _world,  _worldViewDescriptions);
            gridIndicationPresenter.Enable();
            
            await CreateControllableUnit();
            await CreateUnit();
        }
        
        private void Update()
        {
            _world.GameSystems?.Update(Time.deltaTime);
        }

        private async Task CreateControllableUnit()
        {
            var unitDescription = _worldDescription.UnitCollection.First();
            
            var unitModel = new UnitModel
            (
                "unit_0", 
                unitDescription, 
                new Vector2Int(51, 49)
            );
            
            var unitViewDescription = _worldViewDescriptions.UnitViewDescriptions.Get(unitModel.Description.ViewId);
            var unitPrefab = await unitViewDescription.Prefab.LoadAssetAsync().Task;
            var unitView = Instantiate(unitPrefab.GetComponent<UnitView>(), Vector3.zero, Quaternion.identity);
            unitViewDescription.Prefab.ReleaseAsset();
            
            var playerPresenter = new PlayerPresenter(unitModel, unitView, _world);
            playerPresenter.Enable();
        }
        
        
        private async Task CreateUnit()
        {
            var unitDescription = _worldDescription.UnitCollection.Last();
            
            var unitModel = new UnitModel
            (
                "bear", 
                unitDescription, 
                new Vector2Int(51, 50)
            );
            
            var unitViewDescription = _worldViewDescriptions.UnitViewDescriptions.Get(unitModel.Description.ViewId);
            var unitPrefab = await unitViewDescription.Prefab.LoadAssetAsync().Task;
            var unitView = Instantiate(unitPrefab.GetComponent<UnitView>(), Vector3.zero, Quaternion.identity);
            unitViewDescription.Prefab.ReleaseAsset();
            
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