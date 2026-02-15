using Runtime.Common;
using UnityEngine.UIElements;

namespace Runtime.Units.UI
{
    public class UnitUIPresenter : IPresenter
    {
        private readonly UnitModel _model;

        private readonly TextElement _name;

        private readonly TextElement _fraction;

        private readonly VisualElement _healthBarFill;

        private readonly TextElement _healthValue;

        private readonly float _startHealth;

        public UnitUIPresenter(UnitModel model, UnitView view)
        {
            _model = model;

            _startHealth = _model.Health;

            _name = view.UIDocument.rootVisualElement.Q<Label>("name");
            _fraction = view.UIDocument.rootVisualElement.Q<Label>("fraction");
            _healthBarFill = view.UIDocument.rootVisualElement.Q<VisualElement>("health-bar-fill");
            _healthValue = view.UIDocument.rootVisualElement.Q<Label>("health-bar-value");
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
            _healthBarFill.style.width = new Length(health / _startHealth * 100f, LengthUnit.Percent);

            _healthBarFill.parent.style.display = health <= 0f ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private string Capitalize(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}