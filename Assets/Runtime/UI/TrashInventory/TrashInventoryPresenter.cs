using Runtime.Common;
using Runtime.Core;
using Runtime.UI.Inventory;
using Runtime.UI.Inventory.Cells;
using Runtime.UI.Transfer;
using Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace Runtime.UI.TrashInventory
{
    public class TrashInventoryPresenter : IPresenter
    {
        private readonly InventoryView _view;
        private readonly InventoryModel _model;
        private readonly UIContent _uiContent;
        private readonly WorldViewDescriptions _viewDescriptions;
        private readonly World _world;

        public TrashInventoryPresenter(InventoryView view, InventoryModel model, WorldViewDescriptions viewDescriptions, UIContent uiContent, World world)
        {
            _view = view;
            _model = model;
            _viewDescriptions = viewDescriptions;
            _uiContent = uiContent;
            _world = world;
        }
        
        public void Enable()
        {
            _uiContent.GameplayContent.Add(_view.Root);
            var cellAsset = _viewDescriptions.InventoryViewDescription.CellAsset;

            var cellModel = new CellModel();
            _model.Cells.Add(cellModel);

            var cellView = new CellView(cellAsset);
            cellView.Root.AddToClassList("cell-trash");
            _view.CellsContainer.Add(cellView.Root);

            var cellPresenter = new CellPresenter(cellModel, cellView, _viewDescriptions, _world);
            cellPresenter.Enable();
            
            _world.TransferModel.TargetInventory.Value = _model;
            _world.TransferModel.Mode.Value = TransferMode.Trash;

            var callback = new EventCallback<ClickEvent>(_ => _model.CellSelected(cellModel));
            cellView.Root.RegisterCallback(callback);
        }

        public void Disable()
        {
            if (_world.TransferModel.TargetInventory.Value == _model)
            {
                _world.TransferModel.TargetInventory.Value = null;
                _world.TransferModel.Mode.Value = TransferMode.Default;
            }

            _view.Root.RemoveFromHierarchy();
        }
    }
}