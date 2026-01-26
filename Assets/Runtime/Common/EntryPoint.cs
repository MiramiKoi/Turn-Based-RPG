using Runtime.Landscape.Grid;
using Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Runtime.Common
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private WorldViewDescriptions _worldViewDescriptions;
        
        private void Start()
        {
            var gridModel = new GridModel();
            var gridView = new GridView(_tilemap);
            var gridPresenter = new GridPresenter(gridModel, gridView, _worldViewDescriptions);
            gridPresenter.Enable();
        }
    }
}