using Runtime.Landscape.Grid;
using Runtime.Landscape.Grid.Interaction;
using Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.Common
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _gridInteractionTilemap;
        [SerializeField] private WorldViewDescriptions _worldViewDescriptions;
        
        private readonly World _world = new();
        private PlayerControls _playerControls;
        
        private void Start()
        {
            _playerControls = new PlayerControls();
            _playerControls.Enable();
            
            _world.SetData(_playerControls);
            
            var gridView = new GridView(_tilemap);
            var gridPresenter = new GridPresenter(_world.GridModel, gridView, _worldViewDescriptions);
            gridPresenter.Enable();

            var gridInteractionView = new GridInteractionView(_tilemap);
            var gridInteractionPresenter = new GridInteractionPresenter(_world.GridInteractionModel, gridInteractionView, _world);
            gridInteractionPresenter.Enable();
        }
    }
}