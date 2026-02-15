using Runtime.Common;
using Runtime.UI.Inventory.Cells;
using Runtime.Units;

namespace Runtime.Equipment
{
    public class EquipmentPresenter : IPresenter
    {
        private readonly UnitModel _unitModel;
        private readonly EquipmentView _view;

        public EquipmentPresenter(UnitModel unitModel, EquipmentView view)
        {
            _unitModel = unitModel;
            _view = view;
        }

        public void Enable()
        {
            _unitModel.Equipment.Inventory.OnCellChanged += HandleCellChanged;
            UpdateView();
        }

        public void Disable()
        {
            _unitModel.Equipment.Inventory.OnCellChanged -= HandleCellChanged;
        }

        private void HandleCellChanged(CellModel cell)
        {
            UpdateView();
        }

        private void UpdateView()
        {
            if (_unitModel.Equipment.TryGetStats("weapon", out var weaponStats))
            {
                _view.Damage.text = weaponStats["damage"].MaxValue.ToString();
            }
            else
            {
                _view.Damage.text = _unitModel.Stats.Get("attack_damage").Value.ToString();
            }
    
            if (_unitModel.Equipment.TryGetStats("armour", out var armorStats))
            {
                _view.Protection.text = armorStats["protection"].MaxValue.ToString();
            }
            else
            {
                _view.Protection.text = "0";
            }
        }
    }
}