using System.Globalization;
using Runtime.Common;
using UnityEngine.UIElements;

namespace Runtime.Stats
{
    public class StatPresenter : IPresenter
    {
        private readonly StatModel _model;
        private readonly Label _view;

        public StatPresenter(StatModel model, Label view)
        {
            _model = model;
            _view = view;
        }
        
        public void Enable()
        {
            _model.ValueChanged += OnValueChanged;
            OnValueChanged(_model.Value);
        }
        
        public void Disable()
        {
            _model.ValueChanged -= OnValueChanged;
        }
        
        private void OnValueChanged(float value)
        {
            _view.text = value.ToString(CultureInfo.InvariantCulture);
        }
    }
}