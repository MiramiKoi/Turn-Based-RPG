using Runtime.Common;
using Runtime.Core;
using Runtime.ViewDescriptions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UI.Inventory.Cells
{
    public class CellPresenter : IPresenter
    {
        private readonly CellModel _model;
        private readonly CellView _view;
        private readonly WorldViewDescriptions _viewDescriptions;
        private readonly World _world;

        public CellPresenter(CellModel model, CellView view, WorldViewDescriptions viewDescriptions, World world)
        {
            _model = model;
            _view = view;
            _viewDescriptions = viewDescriptions;
            _world = world;
        }

        public void Enable()
        {
            Update();

            _model.OnChanged += Update;
            _model.OnCellSelected += HandleCellSelected;
            _model.OnCellDeselected += HandleCellDeselected;
            _model.OnPriceEnabled += HandlePriceEnabled;
            _model.OnPriceDisabled += HandlePriceDisabled;
        }
        
        public void Disable()
        {
            _model.OnChanged -= Update;
            _model.OnCellSelected -= HandleCellSelected;
            _model.OnCellDeselected -= HandleCellDeselected;
        }

        private async void Update()
        {
            var hasItem = _model.Amount > 0;
            
            if (hasItem)
            {
                _view.Amount.style.display = DisplayStyle.Flex;

                _view.Amount.text = _model.Amount.ToString();
                
                HandlePriceEnabled();
                
                var itemViewDescription = _viewDescriptions.ItemViewDescriptions.Get(_model.ItemDescription.ViewId);
                var loadModel = _world.AddressableModel.Load<Sprite>(itemViewDescription.Icon.AssetGUID);
                await loadModel.LoadAwaiter;

                _view.Icon.style.backgroundImage = loadModel.Result.texture;
            }
            else
            {
                Clear();
            }
        }

        private void HandleCellSelected()
        {
            _view.Root.AddToClassList("cell-selected");
        }
        
        private void HandleCellDeselected()
        {
            _view.Root.RemoveFromClassList("cell-selected");
        }
        
        private void HandlePriceEnabled()
        {
            HandlePriceDisabled();
            
            if (_model.ItemDescription == null || _model.ItemDescription.IsBuyable == false)
            {
                return;
            }
            
            _view.Price.text = _model.ItemDescription.Price.ToString();
            _view.Price.style.display = DisplayStyle.Flex;
        }
        
        private void HandlePriceDisabled()
        {
            _view.Price.style.display = DisplayStyle.None;
        }
        
        private void Clear()
        {
            _view.Icon.style.backgroundImage = null;
            _view.Amount.style.display = DisplayStyle.None;
            _view.Price.style.display = DisplayStyle.None;
        }
    }
}