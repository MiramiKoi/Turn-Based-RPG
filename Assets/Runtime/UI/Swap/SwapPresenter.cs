using Runtime.Common;
using Runtime.UI.Inventory.Cells;

namespace Runtime.UI.Swap
{
    public class SwapPresenter : IPresenter
    {
        private readonly SwapModel _model;

        public SwapPresenter(SwapModel model)
        {
            _model = model;
        }

        public void Enable()
        {
            _model.OnSwap += HandleSwap;
        }

        public void Disable()
        {
            _model.OnSwap -= HandleSwap;
            _model.Clear();
        }

        private void HandleSwap(CellModel targetCell)
        {
            if (_model.SourceCell == null || targetCell == null)
            {
                Clear();
                return;
            }
            
            if (_model.SourceCell == targetCell)
            {
                Clear();
                return;
            }

            var sourceItem = _model.SourceCell.ItemDescription;
            var sourceAmount = _model.SourceCell.Amount;
            var targetItem = targetCell.ItemDescription;
            var targetAmount = targetCell.Amount;
            
            if (targetItem == null)
            {
                _model.SourceCell.TryTake(sourceAmount);
                targetCell.TryPut(sourceItem, sourceAmount);
                Clear();
                return;
            }
            
            if (targetItem.Id == sourceItem.Id)
            {
                var put = targetCell.TryPut(sourceItem, sourceAmount);
                _model.SourceCell.TryTake(put);
                Clear();
                return;
            }
            
            _model.SourceCell.TryTake(sourceAmount);
            targetCell.TryTake(targetAmount);

            _model.SourceCell.TryPut(targetItem, targetAmount);
            targetCell.TryPut(sourceItem, sourceAmount);

            Clear();
        }

        private void Clear()
        {
            _model.Clear();
        }
    }
}