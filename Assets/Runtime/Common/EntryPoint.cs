using System.Collections.Generic;
using System.IO;
using fastJSON;
using Runtime.Descriptions.Units;
using Runtime.Extensions;
using Runtime.Landscape.Grid;
using Runtime.Units;
using Runtime.Landscape.Grid.Indication;
using Runtime.Landscape.Grid.Interaction;
using Runtime.ViewDescriptions;
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
        private PlayerControls _playerControls;
        
        private void Start()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();
            
            _world.SetData(_playerControls);
            
            var gridView = new GridView(_mainTilemap);
            var gridPresenter = new GridPresenter(_world.GridModel, gridView, _worldViewDescriptions);
            gridPresenter.Enable();
            
            var gridInteractionPresenter = new GridInteractionPresenter(_world.GridInteractionModel, gridView, _world);
            gridInteractionPresenter.Enable();

            var gridIndicationView = new GridIndicationView(_indicationTilemap);
            var gridIndicationPresenter = new GridIndicationPresenter(gridIndicationView, _world,  _worldViewDescriptions);
            gridIndicationPresenter.Enable();
            
            CreateUnit();
        }

        private void CreateUnit()
        {
            var unitDescriptionRaw = File.ReadAllText("Assets/Content/Descriptions/Units/units_description.json");
            var unitDescription = JSON.ToObject<Dictionary<string, object>>(unitDescriptionRaw);
            
            var unitModel = new UnitModel
            (
                "unit_0", 
                new UnitDescription("warrior",unitDescription.GetNode("warrior")), 
                new Vector2Int(0, 0)
            );
            
            var unitView = Instantiate(_unitPrefab, Vector3.zero, Quaternion.identity);
            
            var unitPresenter = new UnitPresenter(unitModel, unitView);
            unitPresenter.Enable();
            
            var position = new Vector2Int(50, 50);
            if (_world.GridModel.TryPlace(unitModel, position))
            {
                unitModel.MoveTo(position);
            }
        }
    }
}