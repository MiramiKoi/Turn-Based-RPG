using System.Collections.Generic;
using Runtime.Common;
using Runtime.Stats;
using Runtime.UI.Inventory.Cells;
using Runtime.Units;

namespace Runtime.Equipment
{
    public class EquipmentPresenter : IPresenter
    {
        private readonly UnitModel _unitModel;
        private readonly EquipmentView _view;

        private readonly List<StatPresenter> _stats = new() ;
        
        public EquipmentPresenter(UnitModel unitModel, EquipmentView view)
        {
            _unitModel = unitModel;
            _view = view;
        }

        public void Enable()
        {
            foreach (var pair in _view.Stats)
            {
                var statPresenter = new StatPresenter(_unitModel.Stats[pair.Key], pair.Value);
                statPresenter.Enable();
                _stats.Add(statPresenter);
            }
        }

        public void Disable()
        {
            foreach (var statPresenter in _stats)
            {
                statPresenter.Disable();
            }
            _stats.Clear();
        }
    }
}