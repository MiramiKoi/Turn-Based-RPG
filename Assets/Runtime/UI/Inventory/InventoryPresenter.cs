using System.Collections.Generic;
using Runtime.Common;
using Runtime.Core;
using Runtime.UI.Inventory.Cells;
using Runtime.UI.Transfer;
using Runtime.ViewDescriptions;
using UnityEngine.UIElements;

namespace Runtime.UI.Inventory
{
    public class InventoryPresenter : IPresenter
    {
        private readonly List<CellPresenter> _cellsPresenters = new();
        private readonly InventoryModel _model;
        private readonly InventoryView _view;
        private readonly World _world;
        private readonly WorldViewDescriptions _viewDescriptions;
        private readonly UIContent _uiContent;
        private readonly List<(VisualElement root, EventCallback<ClickEvent> callback)> _callbacks = new();
        private readonly InventoryType _type;

        public InventoryPresenter(InventoryModel model, InventoryView view, WorldViewDescriptions viewDescriptions,
            World world, InventoryType type)
        {
            _model = model;
            _view = view;
            _viewDescriptions = viewDescriptions;
            _uiContent = viewDescriptions.UIContent;
            _world = world;
            _type = type;
        }

        public void Enable()
        {
            _uiContent.GameplayContent.Add(_view.Root);
            var cellAsset = _viewDescriptions.InventoryViewDescription.CellAsset;

            foreach (var cellModel in _model.Cells)
            {
                CreateCell(cellModel, cellAsset);
            }

            _model.Enabled = true;
        }

        public void Disable()
        {
            Clear();

            _model.Enabled = false;
        }

        private void CreateCell(CellModel cellModel, VisualTreeAsset cellAsset)
        {
            var cellView = new CellView(cellAsset);
            _view.CellsContainer.Add(cellView.Root);
            CellStyle(cellView);

            var cellPresenter = new CellPresenter(cellModel, cellView, _viewDescriptions, _world);
            cellPresenter.Enable();

            var callback = new EventCallback<ClickEvent>(_ => CellClicked(cellModel));
            cellView.Root.RegisterCallback(callback);

            _callbacks.Add((cellView.Root, callback));
            _cellsPresenters.Add(cellPresenter);
        }

        private void Clear()
        {
            foreach (var cellPresenter in _cellsPresenters)
            {
                cellPresenter.Disable();
            }

            _cellsPresenters.Clear();
            _view.CellsContainer.Clear();
            _view.Root.RemoveFromHierarchy();

            foreach (var (root, callback) in _callbacks)
            {
                root.UnregisterCallback(callback);
            }

            _callbacks.Clear();
        }

        private void CellStyle(CellView cell)
        {
            switch (_type)
            {
                case InventoryType.Trash:
                    cell.Root.AddToClassList("cell-trash");
                    break;
                case InventoryType.Equipment:
                    cell.Root.AddToClassList("cell-equipment");
                    break;
                default:
                    cell.Root.AddToClassList("cell");
                    break;
            }
        }

        private void CellClicked(CellModel cell)
        {
            _world.TransferRouter.HandleCellSelected(cell, _model, _type);
        }
    }
}