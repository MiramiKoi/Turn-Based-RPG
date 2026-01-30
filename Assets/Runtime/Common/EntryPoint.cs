using fastJSON;
using Runtime.AsyncLoad;
using Runtime.Descriptions;
using Runtime.Descriptions.Surface;
using Runtime.Descriptions.Units;
using Runtime.Extensions;
using Runtime.Input;
using Runtime.Landscape.Grid;
using Runtime.Landscape.Grid.Indication;
using Runtime.Landscape.Grid.Interaction;
using Runtime.LoadSteps;
using Runtime.Player;
using Runtime.Units;
using Runtime.ViewDescriptions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.Common
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Tilemap _mainTilemap;
        [SerializeField] private Tilemap _indicationTilemap;
        [SerializeField] private WorldViewDescriptions _worldViewDescriptions;

        [SerializeField] private UnitView _unitPrefab;

        
        private readonly World _world = new();
        private readonly WorldDescription _worldDescription = new();
        private readonly AddressableModel _addressableModel = new();
        private readonly List<IPresenter> _presenters = new();

        private PlayerControls _playerControls;
        
        private async void Start()
        {
            IStep[] persistentLoadStep =
            {
                new AddressableLoadStep(_addressableModel, _presenters),
                new DescriptionsLoadStep(_worldDescription, _addressableModel),
                //new ViewDescriptionsLoadStep(_worldViewDescriptions, _addressableModel),
            };

            foreach (var step in persistentLoadStep)
            {
                await step.Run();
            }

            _playerControls = new PlayerControls();
            _playerControls.Enable();
            
            _world.SetData(_playerControls, _worldDescription);
            
            var gridView = new GridView(_mainTilemap);
            var gridPresenter = new GridPresenter(_world.GridModel, gridView, _world, _worldViewDescriptions);
            gridPresenter.Enable();
            
            var gridInteractionPresenter = new GridInteractionPresenter(_world.GridInteractionModel, gridView, _world);
            gridInteractionPresenter.Enable();

            var gridIndicationView = new GridIndicationView(_indicationTilemap);
            var gridIndicationPresenter = new GridIndicationPresenter(gridIndicationView, _world,  _worldViewDescriptions);
            gridIndicationPresenter.Enable();
            
            CreateUnit();
        }
        
        private void Update()
        {
            _world.GameSystems?.Update(Time.deltaTime);
        }

        private void CreateUnit()
        {
            var unitDescription = _worldDescription.UnitCollection.First();
            
            var unitModel = new UnitModel
            (
                "unit_0", 
                unitDescription, 
                new Vector2Int(50, 49)
            );
            
            var unitView = Instantiate(_unitPrefab, Vector3.zero, Quaternion.identity);
            
            var playerPresenter = new PlayerPresenter(unitModel, unitView, _world);
            playerPresenter.Enable();
        }
    }
}