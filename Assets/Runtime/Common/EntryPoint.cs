using Runtime.Landscape.Grid;
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
        }
    }
}