using Runtime.Common;
using UnityEngine.UIElements;

namespace Runtime.UI.Inventory.Cells
{
    public class CellPresenter : IPresenter
    {
        private readonly CellModel _model;
        private readonly CellView _view;
        
        public CellPresenter(CellModel model, CellView view)
        {
            _model = model;
            _view = view;
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

        private void Update()
        {
            if (_model.Amount <= 0)
            {
                _view.Amount.style.display = DisplayStyle.None;
                _view.Icon.style.backgroundImage = null;
            }
        }
    }
}