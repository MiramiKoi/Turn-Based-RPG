using System.Collections.Generic;
using Runtime.Common;
using Runtime.UI.Inventory.Cells;
using Runtime.ViewDescriptions;

namespace Runtime.UI.Inventory
{
    public class InventoryPresenter : IPresenter
    {
        private readonly List<CellPresenter> _cellsPresenters = new();
        private readonly InventoryModel _model;
        private readonly InventoryView _view;
        private readonly WorldViewDescriptions _viewDescriptions;
        private readonly UIContent _uiContent;

        public InventoryPresenter(InventoryModel model, InventoryView view, WorldViewDescriptions viewDescriptions, UIContent uiContent)
        {
            _model = model;
            _view = view;
            _viewDescriptions = viewDescriptions;
            _uiContent = uiContent;
        }

        public void Enable()
        {
            _uiContent.GameplayContent.Add(_view.Root);
            var cellAsset = _viewDescriptions.InventoryViewDescription.CellAsset;
            
            foreach (var cellModel in _model.Cells)
            {
                var cellView = new CellView(cellAsset);
                _view.CellsContainer.Add(cellView.Root);
                
                var cellPresenter = new CellPresenter(cellModel, cellView);
                cellPresenter.Enable();
                
                _cellsPresenters.Add(cellPresenter);
            }

            _model.Enabled = true;
        }

        public void Disable()
        {
            foreach (var cellPresenter in _cellsPresenters)
            {
                cellPresenter.Disable();
            }
            
            _cellsPresenters.Clear();
            _view.CellsContainer.Clear();
            
            _view.Root.RemoveFromHierarchy();

            _model.Enabled = false;
        }
    }
}