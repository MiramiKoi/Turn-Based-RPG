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
        }

        public void Disable()
        {
            _model.OnChanged -= Update;
        }

        private async void Update()
        {
            if (_model.Amount > 0)
            {
                _view.Amount.style.display = DisplayStyle.Flex;

                _view.Amount.text = _model.Amount.ToString();

                var itemViewDescription = _viewDescriptions.ItemViewDescriptions.Get(_model.ItemDescription.ViewId);
                var loadModel = _world.AddressableModel.Load<Sprite>(itemViewDescription.Icon.AssetGUID);
                await loadModel.LoadAwaiter;

                _view.Icon.style.backgroundImage = loadModel.Result.texture;
            }
            else
            {
                _view.Amount.style.display = DisplayStyle.None;
                _view.Icon.style.backgroundImage = null;
            }
        }
    }
}