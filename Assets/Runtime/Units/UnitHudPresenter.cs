using Runtime.Common;
using UnityEngine.UIElements;

namespace Runtime.Units
{
    public class UnitHudPresenter : IPresenter
    {
        private readonly UnitView _view;
        
        private readonly UnitModel _model;

        private readonly TextElement _name;
        
        private readonly TextElement _fraction;
        
        private readonly VisualElement _healthBarFill;
        
        private readonly TextElement _healthValue;

        private readonly float _startHealth;
        
        public UnitHudPresenter(UnitModel model, UnitView view)
        {
            _model = model;
            _view = view;
            
            _startHealth = _model.Health;
            
            _name = _view.UIDocument.rootVisualElement.Q<Label>("name");
            _fraction = _view.UIDocument.rootVisualElement.Q<Label>("fraction");
            _healthBarFill = _view.UIDocument.rootVisualElement.Q<VisualElement>("health-bar-fill");
            _healthValue = _view.UIDocument.rootVisualElement.Q<Label>("health-bar-value");
        }

        public void Enable()
        {
            _model.Stats["health"].ValueChanged += OnChangeHealth;
            
            _name.text = Capitalize(_model.Description.ViewId);
            _fraction.text = Capitalize(_model.Description.Fraction);
            
            OnChangeHealth(_model.Health);
        }

        public void Disable()
        {
            _model.Stats["health"].ValueChanged -= OnChangeHealth;
        }


        private void OnChangeHealth(float health)
        {
            _healthValue.text = $"{_model.Health}/{_startHealth}";  
            _healthBarFill.style.width = new Length((health / _startHealth) * 100f, LengthUnit.Percent);
            
            _healthBarFill.parent.style.display = health <= 0f ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private string Capitalize(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}